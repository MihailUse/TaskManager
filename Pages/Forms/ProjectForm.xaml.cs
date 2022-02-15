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
using TaskManager.Windows;

namespace TaskManager.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для ProjectForm.xaml
    /// </summary>
    public partial class ProjectForm : Page
    {
        private List<User> Memberships = new List<User>();
        public ProjectForm(bool isNewProject)
        {
            InitializeComponent();

            if (isNewProject)
                this.CreateButton.Visibility = Visibility.Visible;
            else
                this.SaveButton.Visibility = Visibility.Visible;

            Memberships.Add(FrameManager.User);
            this.MembershipList.ItemsSource = Memberships;
        }

        private void SelectLogoButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveProjectButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
