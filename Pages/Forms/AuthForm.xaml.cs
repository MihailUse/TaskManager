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

namespace TaskManager.Pages.Forms
{
	/// <summary>
	/// Логика взаимодействия для AuthForm.xaml
	/// </summary>
	public partial class AuthForm : Page
	{
		private TaskManagerEntities DataBaseContext;

		public AuthForm(ref TaskManagerEntities DataBaseContext)
		{
			InitializeComponent();
			this.DataBaseContext = DataBaseContext;
		}

		private void AuthButton_Click(object sender, RoutedEventArgs e)
		{
			string login = LoginField.Text.Trim();
			string password = PasswordField.Text.Trim();

			User user = DataBaseContext.User.Where(x => x.login.Equals(login)).FirstOrDefault();

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

			NavigationService.Navigate(new MainPage(ref DataBaseContext, 1));
		}

		private void RegistrationButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new RegistrationForm(ref DataBaseContext));
		}

		private void ShowError(string error)
		{
			ErrorField.Visibility = Visibility.Visible;
			ErrorField.Text = error;
		}
	}
}
