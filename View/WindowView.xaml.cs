using System.Windows;
using System.Windows.Input;

namespace TaskManager.View
{
    public partial class WindowView : Window
    {
        public WindowView()
        {
            InitializeComponent();
        }

        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;

            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
