using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Project
    {
        public int Id {get; set; }
        [Required]
        public string Title {get; set;} = string.Empty;
        public int DeadLine {get; set; }
        public ICollection<Tasky> Tasks {get; set; }
        // public ICollection<User> Users {get; set; }
    }
}