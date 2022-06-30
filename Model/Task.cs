using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Model
{
    public class Task
    {
        [Key]
        public long Id { get; set; }

        [
            Index(IsUnique = true),
            Required(ErrorMessage = "Title required"),
            MinLength(3, ErrorMessage = "Title must be at least 3 character"),
            MaxLength(256, ErrorMessage = "Title must be no more than 256 characters")
        ]
        public string Title { get; set; }

        [StringLength(16384, ErrorMessage = "Description must be no more than 16384 characters")]
        public string Description { get; set; }


        [Required, ForeignKey("Project")]
        public long ProjectId { get; set; }

        [Required, ForeignKey("Status")]
        public int StatusId { get; set; }

        [Required, ForeignKey("User")]
        public long UserId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
    }
}
