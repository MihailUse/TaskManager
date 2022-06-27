using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Model
{
    public class Membership
    {
        [Key]
        public long Id { get; set; }

        [Required, ForeignKey("User")]
        public long UserId { get; set; }

        [Required, ForeignKey("Project")]
        public long ProjectId { get; set; }

        [Required, ForeignKey("Role")]
        public int RoleId { get; set; }


        public virtual Project Project { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
