using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Model
{
    public class Status
    {
        public Status() { }

        public Status(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [Required, MinLength(3), MaxLength(256)]
        public string Name { get; set; }


        public virtual List<Task> Tasks { get; set; }
    }
}
