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
using TaskManager.Pages.Forms;
using TaskManager.Windows;

namespace TaskManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProjectsPage.xaml
    /// </summary>
    public partial class ProjectsPage : Page
    {

        public ProjectsPage()
        {
            InitializeComponent();

            LoadProjects();
        }

        private void LoadProjects()
        {
            var projects = FrameManager.DataBaseContext.Membership
                .Where(x => x.user_id == FrameManager.User.id)
                .Select(x => x.Project)
                .OrderBy(x => x.createdAt)
                .Take(50);

            this.ProjectsList.ItemsSource = projects.ToList();
        }

        private void LoadTasks()
        {
            var tasks = FrameManager.DataBaseContext.Assignment
                .Where(x => x.membership_id == FrameManager.User.id)
                .Select(x => x.Task)
                .OrderBy(x => x.updatedAt)
                .Take(100);
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.WorkSpaceFrame.Navigate(new ProjectForm(isNewProject: true));
        }
    }
}
