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

namespace TaskManager.Pages
{
	/// <summary>
	/// Логика взаимодействия для TasksPage.xaml
	/// </summary>
	public partial class TasksPage : Page
	{
		private TaskManagerEntities DataBaseContext;

		public TasksPage(ref TaskManagerEntities DataBaseContext, int userId)
		{
			InitializeComponent();
			UserName.Text = userId.ToString();
			this.DataBaseContext = DataBaseContext;
		}
	}
}
