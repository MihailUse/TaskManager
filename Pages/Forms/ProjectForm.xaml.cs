using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
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
        private byte[] projectLogo;
        private ObservableCollection<User> SearchableList = new ObservableCollection<User>();
        private ObservableCollection<User> MembershipList = new ObservableCollection<User>();

        public ProjectForm()
        {
            InitializeComponent();

            this.MembershipComboBox.ItemsSource = SearchableList;
            this.MembershipListBox.ItemsSource = MembershipList;

            this.SaveButton.Content = "Create project";
        }

        public ProjectForm(int projectId)
        {
            InitializeComponent();

            this.MembershipComboBox.ItemsSource = SearchableList;
            //this.MembershipListBox.ItemsSource = MembershipList;

            this.project = FrameManager.DataBaseContext.Project
                .Include(x => x.Membership)
                .Where(x => x.id == projectId)
                .First();

            this.NameField.Text = this.project.name;
            this.DescriptionField.Text = this.project.description;
            this.projectLogo = this.project.logo;

            LoadImage(this.projectLogo);
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
                this.projectLogo = imageBytes;

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
            #endregion

            // if project is null then it`s new project
            if (this.project == null)
            {
                Project project = FrameManager.DataBaseContext.Project
                    .Where(x => x.name.Equals(name) && !x.detetedAt.HasValue)
                    .FirstOrDefault();

                if (project != null)
                {
                    ShowError("This name already exists");
                    return;
                }

                Project newProject = new Project()
                {
                    name = name,
                    description = description,
                    logo = this.projectLogo,
                };

                FrameManager.DataBaseContext.Project.Add(newProject);
            }
            else
            {

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

        private void MembershipField_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string userLogin = MembershipComboBox.Text.Trim();
            string likeExpression = $"%{userLogin}%";

            if (userLogin != String.Empty)
            {
                SearchableList.Clear();

                // select users by login
                List<User> users = FrameManager.DataBaseContext.User
                    .Where(x =>
                        DbFunctions.Like(x.login, likeExpression) &&
                        x.id != FrameManager.User.id &&
//                        MembershipList.Contains(x) &&
                        x.detetedAt.HasValue == false
                    )
                    .ToList();

                users.ForEach(x => SearchableList.Add(x));
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

            User user = FrameManager.DataBaseContext.User.Local
                .Where(x => x.id == int.Parse(button.Tag.ToString()))
                .First();

            MembershipList.Remove(user);
        }

        private void MembershipComboBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.IsDropDownOpen = true;
        }
    }
}
