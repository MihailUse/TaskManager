using System.Windows;
using TaskManager.DataBase;
using TaskManager.Pages.Forms;
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
            FrameManager.MainFrame.Navigate(new AuthForm());

            Closing += MainWindow_Closing;
        }

        // close database on window closing
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FrameManager.DataBaseContext.Dispose();
        }
    }
}
