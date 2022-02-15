using System;
using System.Collections.Generic;
using System.Drawing;
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
using ImageGenerator;
using System.IO;
using System.Drawing.Imaging;
using TaskManager.Utils;
using TaskManager.Windows;
using TaskManager.DataBase;
using TaskManager.Pages.Layouts;

namespace TaskManager.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Page
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginField.Text.Trim();
            string password = PasswordField.Text.Trim();
            string repeatPassword = PasswordRepeatField.Text.Trim();

            if (!password.Equals(repeatPassword))
            {
                ShowError("Password mismatch");
                return;
            }

            // check if user already exists
            int existUsers = FrameManager.DataBaseContext.User.Where(x => x.login.Equals(login)).Count();
            if (existUsers != 0)
            {
                ShowError("This login already exists");
                return;
            }

            // create new user
            Bitmap avatar = Generator.ResizeImage(Generator.GenerateImage(), 256, 256);
            User user = new User()
            {
                login = login,
                password = password,
                avatar = ImageConvertor.BitmapToBytes(avatar),
            };

            User newUser = FrameManager.DataBaseContext.User.Add(user);
            try
            {
                FrameManager.DataBaseContext.SaveChanges();
            }
            catch (Exception error)
            {
                ShowError($"Internal error: {error}");
                return;
            }

            FrameManager.User = newUser;
            FrameManager.MainFrame.Navigate(new MainPage());
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AuthForm());
        }

        private void ShowError(string error)
        {
            ErrorField.Visibility = Visibility.Visible;
            ErrorField.Text = error;
        }
    }
}
