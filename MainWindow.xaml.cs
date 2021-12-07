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
using TaskManager.Pages;
using TaskManager.Pages.Forms;

namespace TaskManager
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private TaskManagerEntities DataBaseContext = new TaskManagerEntities();

		public MainWindow()
		{
			InitializeComponent();
			//MainFrame.Navigate(new AuthForm(ref DataBaseContext));

			MainFrame.Navigate(new MainPage(ref DataBaseContext, DataBaseContext.User.Find(8)));

		}

		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DataBaseContext.Dispose();
		}
	}
}
