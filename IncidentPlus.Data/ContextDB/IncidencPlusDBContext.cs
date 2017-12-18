using IncidentPlus.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Data.ContextDB
{
    public class IncidencPlusDBContext : DbContext
    {
        public IncidencPlusDBContext():base("IncidentPlusConString"){}

        public DbSet<Rol> Roles{ get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Level> Levels { get; set; }



    }
}
