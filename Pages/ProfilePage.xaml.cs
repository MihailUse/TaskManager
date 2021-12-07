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
	/// Логика взаимодействия для ProfilePage.xaml
	/// </summary>
	public partial class ProfilePage : Page
	{
		private TaskManagerEntities DataBaseContext;
		private User user;

		public ProfilePage(ref TaskManagerEntities DataBaseContext, int userId)
		{
			InitializeComponent();
			this.DataBaseContext = DataBaseContext;
			this.user = DataBaseContext.User.Find(userId);



			//UserName.Text = "Task Manager " + user.id;
			//UserAvatar.Source = BytesToImage(user.avatar);
		}
	}
}
