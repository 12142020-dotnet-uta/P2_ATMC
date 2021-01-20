using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceBook.Models;

namespace SpaceBook.Repository
{
    class ApplicationDbContext :DbContext
    {
        public DbSet<Comment>  Comments{ get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Follow> Follows{ get; set; }
        public DbSet<Message> Messages{ get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Rating> Ratings{ get; set; }
        public DbSet<SubComment> SubComments{ get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<UserPicture> UserPictures{ get; set; }


        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-0IVLTFI;Database=P2_NewTest1;Trusted_Connection=True");

            }

        }
    }
}
