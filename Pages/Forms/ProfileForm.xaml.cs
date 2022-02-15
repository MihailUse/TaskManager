using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Windows;

namespace TaskManager.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для ProfileForm.xaml
    /// </summary>
    public partial class ProfileForm : Page
    {
        public ProfileForm()
        {
            InitializeComponent();

            this.DataContext = FrameManager.User;
        }

        private void SelectAvatarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            openFileDialog.Title = "Select file to upload";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                FrameManager.User.avatar = imageBytes;

                using (MemoryStream ms = new MemoryStream(imageBytes, false))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();

                    this.Avatar.Source = bitmapImage;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var user = FrameManager.DataBaseContext.User.Attach(FrameManager.User);
            FrameManager.DataBaseContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
            FrameManager.DataBaseContext.SaveChanges();
            FrameManager.WorkSpaceFrame.Navigate(new ProfilePage());
        }
    }
}
