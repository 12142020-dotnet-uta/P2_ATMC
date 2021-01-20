using System;
using SpaceBook.Models;
using Microsoft.EntityFrameworkCore;

namespace SpaceBook.Repository
{         
    public class ApplicationDbContext : DbContext
    {
        DbSet<Comment> Comments { get; set; }
        DbSet<Favorite> Favorites { get; set; }
        DbSet<Follow> Follows { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Picture> Pictures { get; set; }
        DbSet<Rating> Ratings { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserPicture> UserPictures { get; set; }
        //DbSet<MessageRecipient> MessageRecipients { get; set; }
        //DbSet<Follower> Followers { get; set; }


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
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SpaceBookTest;Trusted_Connection=True;");
                base.OnConfiguring(optionsBuilder);
            }
        }

    }
}
