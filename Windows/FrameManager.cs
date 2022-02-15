using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TaskManager.DataBase;

namespace TaskManager.Windows
{
    static class FrameManager
    {
        public static Frame MainFrame { get; set; }
        public static Frame WorkSpaceFrame { get; set; }
        public static TaskManagerEntities DataBaseContext { get; set; }
        public static User User { get; set; }
    }
}
