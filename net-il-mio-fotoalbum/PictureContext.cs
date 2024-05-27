using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using net_il_mio_fotoalbum.Models;

namespace la_mia_pizzeria_static
{
    public class PictureContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=FotografoDB;" +
            "Integrated Security=True;TrustServerCertificate=True");
        }
    }

}

