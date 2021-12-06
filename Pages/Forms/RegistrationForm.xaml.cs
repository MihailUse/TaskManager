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
using TaskManager.DataBase;
using ImageGenerator;
using System.IO;
using System.Drawing.Imaging;

namespace TaskManager.Pages.Forms
{
	/// <summary>
	/// Логика взаимодействия для RegistrationForm.xaml
	/// </summary>
	public partial class RegistrationForm : Page
	{
		private TaskManagerEntities DataBaseContext;

		public RegistrationForm(ref TaskManagerEntities DataBaseContext)
		{
			InitializeComponent();
			this.DataBaseContext = DataBaseContext;
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

			int existUsers = DataBaseContext.User.Where(x => x.login.Equals(login)).Count();

			if (existUsers != 0)
			{
				ShowError("This login already exists");
				return;
			}

			Bitmap avatar = Generator.ResizeImage(Generator.GenerateImage(), 256, 256);
			byte[] avatarBytes;

			using (MemoryStream ms = new MemoryStream())
			{
				avatar.Save(ms, ImageFormat.Png);
				avatarBytes = ms.ToArray();
			}

			User user = new User()
			{
				login = login,
				password = password,
				avatar = avatarBytes,
			};

			DataBaseContext.User.Add(user);
			DataBaseContext.SaveChanges();

			NavigationService.Navigate(new MainPage(ref DataBaseContext, user));
		}

		private void AuthButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new AuthForm(ref DataBaseContext));
		}

		private void ShowError(string error)
		{
			ErrorField.Visibility = Visibility.Visible;
			ErrorField.Text = error;
		}
	}
}
