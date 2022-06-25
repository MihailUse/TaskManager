using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Model
{
    public class Role
    {
        public Role() { }

        public Role(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [Required, MinLength(3), MaxLength(256)]
        public string Name { get; set; }

        [Index(IsUnique = true)]
        [Required, MinLength(3), MaxLength(1024)]
        public string Description { get; set; }


        public virtual List<Membership> Membership { get; set; }
    }
}
