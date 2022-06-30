using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Model
{
    public class User
    {
        [Key]
        public long Id { get; set; }

        [
            Index(IsUnique = true),
            Required(ErrorMessage = "Login required"),
            MinLength(3, ErrorMessage = "Login must be at least 3 character"),
            MaxLength(32, ErrorMessage = "Login must be no more than 32 characters"),
            RegularExpression(@"[a-zA-Zа-яА-Я0-9|\s|_]+", ErrorMessage = "Login cannot contain special characters")
        ]
        public string Login { get; set; }

        [
            Required(ErrorMessage = "Password required"),
            MinLength(3, ErrorMessage = "Password must be at least 3 character"),
            MaxLength(256, ErrorMessage = "Password must be no more than 256 characters")
        ]
        public string Password { get; set; }

        [MaxLength(16384, ErrorMessage = "About must be no more than 16384 characters")]
        public string About { get; set; }

        [Required]
        public byte[] Avatar { get; set; }


        public virtual List<Task> Task { get; set; }
        public virtual List<Membership> Membership { get; set; }
    }
}
