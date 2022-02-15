using System;
using System.Collections.Generic;
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
using TaskManager.DataBase;
using TaskManager.Pages.Layouts;
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

            User user = FrameManager.DataBaseContext.User.Where(x => x.login.Equals(login)).FirstOrDefault();

            if (user == null)
            {
                ShowError("Wrong login");
                return;
            }

            if (!user.password.Equals(password))
            {
                ShowError("Wrong password");
                return;
            }

            FrameManager.User = user;
            FrameManager.MainFrame.Navigate(new MainPage());
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
