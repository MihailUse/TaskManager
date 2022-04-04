using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Windows;
using TaskManager.DataBase;
using TaskManager.Pages.Layouts;
using ImageLibrary;
using System.Drawing;
using Isopoh.Cryptography.Argon2;

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

            #region validation
            if (login.Length < 3 && login.Length > 256)
            {
                ShowError("login cannot contain less than 3 and more than 256 characters");
            }

            if (password.Length < 4 && password.Length > 256)
            {
                ShowError("password cannot contain less than 3 and more than 256 characters");
            }
            #endregion

            // check if user already exists
            var existsUser = FrameManager.DataBaseContext.User
                .Where(x => x.login.Equals(login) && !x.detetedAt.HasValue)
                .FirstOrDefault();

            if (existsUser != null)
            {
                ShowError("This login already exists");
                return;
            }

            // generate password hash
            string passwordHash = Argon2.Hash(password, timeCost: 10, parallelism: Environment.ProcessorCount, hashLength: 128);

            // create new user
            Bitmap avatar = ImageConvertor.ResizeImage(ImageGenerator.GenerateImage(), 256, 256);
            User user = new User()
            {
                login = login,
                password = passwordHash,
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
