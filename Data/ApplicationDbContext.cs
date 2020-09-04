// using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPrimerWebApi.Models;

namespace MyPrimerWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var roleAdmin = new IdentityRole()
            {
                Id = "70b19522-8186-41b2-89e1-e87d9bfa5d8d",
                Name = "Admin",
                NormalizedName = "Admin"
            };

            builder.Entity<IdentityRole>().HasData(roleAdmin);

            base.OnModelCreating(builder);
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
    }
}