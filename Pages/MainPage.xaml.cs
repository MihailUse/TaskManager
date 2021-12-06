using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
	/// Логика взаимодействия для MainPage.xaml
	/// </summary>
	public partial class MainPage : Page
	{
		private TaskManagerEntities DataBaseContext;
		private User user;

		public MainPage(ref TaskManagerEntities DataBaseContext, User user)
		{
			InitializeComponent();
			this.DataBaseContext = DataBaseContext;
			this.user = user;

			UserName.Text = "Task Manager " + user.id;
			UserAvatar.Source = BytesToImage(user.avatar);
			WorkSpaceFrame.Navigate(new ProjectsPage(ref DataBaseContext));
		}

		private BitmapImage BytesToImage(byte[] bytes)
		{
			using (MemoryStream ms = new MemoryStream(bytes))
			{
				ms.Position = 0;
				BitmapImage image = new BitmapImage();
				image.BeginInit();
				image.StreamSource = ms;
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.EndInit();

				return image;
			}
		}
	}
}
