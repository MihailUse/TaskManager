using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using TaskManager.DataBase;
using TaskManager.Pages.Layouts;
using TaskManager.Utils;
using TaskManager.Windows;

namespace TaskManager.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для ProjectForm.xaml
    /// </summary>
    public partial class ProjectForm : Page
    {
        private Project project;
        private ObservableCollection<User> SearchableList = new ObservableCollection<User>();
        private ObservableCollection<User> MembershipList = new ObservableCollection<User>();

        public ProjectForm()
        {
            InitializeComponent();

            this.MembershipComboBox.ItemsSource = SearchableList;
            this.MembershipListBox.ItemsSource = MembershipList;
            this.project = new Project();
            this.SaveButton.Content = "Create project";
        }

        public ProjectForm(int projectId)
        {
            InitializeComponent();

            this.MembershipComboBox.ItemsSource = SearchableList;
            this.MembershipListBox.ItemsSource = MembershipList;

            this.project = FrameManager.DataBaseContext.Project
                .Include(x => x.Membership)
                .Where(x => x.id == projectId)
                .First();

            this.NameField.Text = this.project.name;
            this.DescriptionField.Text = this.project.description;

            LoadImage(this.project.logo);
            this.SaveButton.Content = "Save project";
        }

        private void SelectLogoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            openFileDialog.Title = "Select file to upload";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                this.project.logo = imageBytes;
                this.ProjectImage.Source = LoadImage(imageBytes);
            }
        }

        private BitmapImage LoadImage(byte[] imageBytes)
        {
            using (MemoryStream ms = new MemoryStream(imageBytes, false))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        // method for save or create project with validation
        private void SaveProjectButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameField.Text.Trim();
            string description = DescriptionField.Text.Trim();

            #region validation
            if (name.Length < 4 || name.Length > 256)
            {
                ShowError("name cannot contain less than 4 and more than 256 characters");
                return;
            }

            if (description.Length > 12000)
            {
                ShowError("description cannot contain more than 12000 characters");
                return;
            }

            Regex regex = new Regex(@"[^a-zа-я1-9|\s|_]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (regex.IsMatch(name))
            {
                ShowError("invalid name characters");
                return;
            }

            Project existsProject = FrameManager.DataBaseContext.Project
                .Where(x => x.name.Equals(name) && !x.detetedAt.HasValue)
                .FirstOrDefault();

            if (existsProject != null)
            {
                ShowError("This name already exists");
                return;
            }
            #endregion

            this.project.name = name;
            this.project.description = description;

            FrameManager.DataBaseContext.Project.Add(this.project);

            try
            {
                FrameManager.DataBaseContext.SaveChanges();
            }
            catch (Exception error)
            {
                ShowError($"Internal error: {error}");
            }

            List<Membership> userIds = this.project.Membership
                .ToList();

            // remove exists memberships if they are not in MembershipList
            foreach (Membership membership in userIds)
            {
                if (!MembershipList.Select(x => x.id).Contains(membership.user_id))
                {
                    this.project.Membership.Remove(membership);
                }
            }

            // add new memberships to database
            foreach (User user in MembershipList)
            {
                Membership newMembership = new Membership()
                {
                    user_id = user.id,
                    project_id = this.project.id,
                };

                this.project.Membership.Add(newMembership);
            }

            try
            {
                FrameManager.DataBaseContext.SaveChanges();
            }
            catch (Exception error)
            {
                ShowError($"Internal error: {error}");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new MainPage());
        }

        private void ShowError(string error)
        {
            ErrorField.Visibility = Visibility.Visible;
            ErrorField.Text = error;
        }

        #region methods for search and select memberships  
        // method for search users and display in searchable list
        private void MembershipField_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string userLogin = MembershipComboBox.Text.Trim();
            string likeExpression = $"%{userLogin}%";

            ComboBox comboBox = sender as ComboBox;

            if (userLogin != String.Empty)
            {
                SearchableList.Clear();

                // get ids list with selected users for exclude in search
                List<int> membershipListIds = MembershipList
                    .Select(x => x.id)
                    .ToList();

                // select users by login
                List<User> users = FrameManager.DataBaseContext.User
                    .Where(x =>
                        DbFunctions.Like(x.login, likeExpression) &&
                        x.id != FrameManager.User.id &&
                        !membershipListIds.Contains(x.id) &&
                        x.detetedAt.HasValue == false
                    )
                    .ToList();

                if (users.Count > 0)
                {
                    comboBox.IsDropDownOpen = true;
                    users.ForEach(x => SearchableList.Add(x));
                }
            }
            else
            {
                comboBox.IsDropDownOpen = false;
            }
        }

        // add user to memberships 
        private void MembershipComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.SelectedItem != null)
            {
                User user = comboBox.SelectedItem as User;

                MembershipList.Add(user);
                SearchableList.Clear();

                MembershipComboBox.Text = String.Empty;
            }
        }

        // remove user from membership list 
        private void RemoveMembershipButton_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image button = sender as Image;

            User user = MembershipList
                .Where(x => x.id == int.Parse(button.Tag.ToString()))
                .First();

            MembershipList.Remove(user);
        }
        #endregion
    }
}
