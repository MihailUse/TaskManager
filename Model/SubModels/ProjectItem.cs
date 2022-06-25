using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Model.Database;

namespace TaskManager.Model.SubModels
{
    [NotMapped]
    internal class ProjectItem : Project
    {
        public int UserCount { get; set; }
        public int TaskCount { get; set; }
        public Roles Role { get; set; }
    }
}
