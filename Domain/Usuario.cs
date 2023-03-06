using System;
using System.Collections.Generic;
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
        // public Guid id {get; set;}
        // public string Username {get; set;}
        public string Email {get; set;}
        // public string Password {get; set;}
        public Rol Rol {get; set;}

        public ICollection<Tasky> Tasks {get; set;}
        // public ICollection<Project> Projects {get; set;}


    }
}