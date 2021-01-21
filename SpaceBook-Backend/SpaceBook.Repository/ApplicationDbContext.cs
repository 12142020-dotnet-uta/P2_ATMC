using System;
using SpaceBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SpaceBook.Repository
{         
    public class ApplicationDbContext : IdentityDbContext
    {
        DbSet<Comment> Comments { get; set; }
        DbSet<Favorite> Favorites { get; set; }
        DbSet<Follow> Follows { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Picture> Pictures { get; set; }
        DbSet<Rating> Ratings { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<UserPicture> UserPictures { get; set; }


        public ApplicationDbContext()
        {}


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=tcp:atmc.database.windows.net,1433;Initial Catalog=atmcdb;Persist Security Info=False;User ID=proj2;Password=password123!;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Proj2Test;Trusted_Connection=True;MultipleActiveResultSets=true");
                base.OnConfiguring(optionsBuilder);
            }
        }

    }
}
