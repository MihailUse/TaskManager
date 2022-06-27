using Isopoh.Cryptography.Argon2;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TaskManager.DataBase;
using TaskManager.Windows;

namespace TaskManager.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для AuthForm.xaml
    /// </summary>
    public partial class AuthForm : Page
    {
        public AuthForm()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginField.Text.Trim();
            string password = PasswordField.Text.Trim();

            #region validation
            if (login.Length < 3 || login.Length > 256)
            {
                ShowError("login cannot contain less than 3 and more than 256 characters");
                return;
            }

            if (password.Length < 4 || password.Length > 256)
            {
                ShowError("password cannot contain less than 3 and more than 256 characters");
                return;
            }
            #endregion

            User user = FrameManager.DataBaseContext.User
                .Where(x => x.login.Equals(login) && !x.detetedAt.HasValue)
                .FirstOrDefault();

            if (user == null)
            {
                ShowError("Wrong login");
                return;
            }

            if (Argon2.Verify(user.password, password))
            {
                FrameManager.User = user;
                FrameManager.MainFrame.Navigate(new MainPage());
                return;
            }

            ShowError("Wrong password");
            return;
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new RegistrationForm());
        }

        private void ShowError(string error)
        {
            ErrorField.Visibility = Visibility.Visible;
            ErrorField.Text = error;
        }
    }
}
