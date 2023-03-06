using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class LuxonDB : DbContext
    {
        public LuxonDB(DbContextOptions<LuxonDB> options) : base(options)
        {}
        public DbSet<Tasky> Tasks { get; set;}
        public DbSet<Usuario> Usuarios { get; set;}
    }   
}