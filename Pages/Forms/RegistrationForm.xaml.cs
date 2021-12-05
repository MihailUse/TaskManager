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
			NavigationService.Navigate(new MainPage(ref DataBaseContext, 1));
		}

		private void AuthButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new AuthForm(ref DataBaseContext));
		}
	}
}
