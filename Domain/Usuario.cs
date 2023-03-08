using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public enum Rol {
        LEAD,
        MID,
        JR,
    }
    public class Usuario : IdentityUser
    {
        // public Guid Id {get; set;}
        // public string Username {get; set;}
        [Required]
        public string Email {get; set;}
        // public string Password {get; set;}
        [Required]

        public Rol Rol {get; set;}
        [Required]

        public ICollection<Tasky> Tasks {get; set;}
        // public ICollection<Project> Projects {get; set;}


    }
}