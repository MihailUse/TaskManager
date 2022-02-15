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
using TaskManager.Pages.Layouts;
using TaskManager.Windows;

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameManager.DataBaseContext = new TaskManagerEntities();
            FrameManager.MainFrame = this.MainFrame;
            //FrameManager.MainFrame.Navigate(new MainPage());
            FrameManager.MainFrame.Navigate(new AuthForm());

            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FrameManager.DataBaseContext.Dispose();
        }
    }
}
