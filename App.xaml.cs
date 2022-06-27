using System.Windows;
using TaskManager.View;
using TaskManager.ViewModel;

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new WindowView();
            MainWindow.DataContext = new WindowViewModel(MainWindow);
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
