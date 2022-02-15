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
using TaskManager.Pages.Forms;
using TaskManager.Windows;

namespace TaskManager.Pages.Layouts
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();

            FrameManager.WorkSpaceFrame = this.WorkSpaceFrame;
            FrameManager.WorkSpaceFrame.Navigate(new ProjectsPage());
        }

        private void PageMenu_DataContextChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem item = comboBox.SelectedItem as ComboBoxItem;
            string action = item.Tag.ToString();

            switch (action)
            {
                case "Projects":
                    FrameManager.WorkSpaceFrame.Navigate(new ProjectsPage());
                    break;

                case "Profile":
                    FrameManager.WorkSpaceFrame.Navigate(new ProfilePage());
                    break;

                case "Settings":
                    MessageBox.Show("Settings");
                    //FrameManager.WorkSpaceFrame.Navigate(new ProfilePage());
                    break;

                case "Logout":
                    FrameManager.User = null;
                    FrameManager.MainFrame.Navigate(new AuthForm());
                    break;

                case "Exit":
                    MessageBox.Show("Test");
                    Application.Current.Shutdown();
                    break;

                default:
                    break;
            }
        }
    }
}
