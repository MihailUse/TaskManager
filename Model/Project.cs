using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Model
{
    public class Project
    {
        [Key]
        public long Id { get; set; }

        [
            Index(IsUnique = true),
            Required(ErrorMessage = "Name required"),
            MinLength(3, ErrorMessage = "Name must be at least 3 character"),
            MaxLength(32, ErrorMessage = "Name must be no more than 32 characters"),
            RegularExpression(@"[a-zA-Zа-яА-Я0-9|\s|_]+", ErrorMessage = "Name cannot contain special characters")
        ]
        public string Name { get; set; }

        [MaxLength(16384, ErrorMessage = "Description must be no more than 16384 characters")]
        public string Description { get; set; }

        [Required]
        public byte[] Logo { get; set; }


        public virtual List<Membership> Membership { get; set; }
        public virtual List<Task> Task { get; set; }
    }
}
