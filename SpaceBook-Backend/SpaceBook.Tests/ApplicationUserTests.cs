using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using SpaceBook.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SpaceBook.Tests
{
    public class ApplicationUserTests
    {
        [Fact]
        public void CheckUserAddToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "13").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                ApplicationUserRepository userRepo = new ApplicationUserRepository(context);

                //AttemptAddApplicationUser returns true if adding to db is successful
                Assert.True(userRepo.AttemptAddApplicationUser(user).Result);
            }

            using (var context = new ApplicationDbContext(options))
            {
                ApplicationUserRepository userRepo = new ApplicationUserRepository(context);

                //Use IsIserInDB to check that the user was added to the DB
                Assert.True(userRepo.IsUserInDb(user.Id).Result);
                context.Database.EnsureDeleted();
            }


        }

        
        [Fact]
        public void CheckGetUserFromDatabaseById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "14").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                ApplicationUserRepository userRepo = new ApplicationUserRepository(context);

                //AttemptAddApplication User returns true if adding to db is successful
                bool test = userRepo.AttemptAddApplicationUser(user).Result;

                //Check to see that the List of ratings you created is the same as the list retrieved from the db
                Assert.Equal(user, userRepo.GetUserById(user.Id).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckGetUserFromDatabaseByUsername()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "15").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                ApplicationUserRepository userRepo = new ApplicationUserRepository(context);

                //AttemptAddApplication User returns true if adding to db is successful
                bool test = userRepo.AttemptAddApplicationUser(user).Result;

                //Check to see that the List of ratings you created is the same as the list retrieved from the db
                Assert.Equal(user, userRepo.GetUserByUsername(user.UserName).Result);
                context.Database.EnsureDeleted();
            }

        }

        

        [Fact]
        public void CheckEditUserInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "16").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                ApplicationUserRepository userRepo = new ApplicationUserRepository(context);

                //AttemptAddApplication User returns true if adding to db is successful
                bool test = userRepo.AttemptAddApplicationUser(user).Result;

                //verify the user in db is the same as the current user
                Assert.Equal(user, userRepo.GetUserById(user.Id).Result);

                //change username
                user.UserName = "UpdatedUsername";

                //Attempts to edit the user in the DB and returns true if successful
                Assert.True(userRepo.AttemptEditApplicationUser(user).Result);

                //Get the user in the DB by the ID and verify the username has changed to UpdatedUsername
                Assert.True(userRepo.GetUserById(user.Id).Result.UserName == "UpdatedUsername");
            }

        }

        [Fact]
        public void CheckDeleteUserInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "17").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                ApplicationUserRepository userRepo = new ApplicationUserRepository(context);

                //AttemptAddApplication User returns true if adding to db is successful
                bool test = userRepo.AttemptAddApplicationUser(user).Result;

               
                //Show that the user was added
                Assert.True(userRepo.IsUserInDb(user.Id).Result);

                //Attempts to delete the rating in the DB and returns true if successful
                Assert.True(userRepo.AttemptRemoveApplicationUser(user.Id).Result);

                //verify the rating is no longer in the db
                Assert.True(userRepo.IsUserInDb(user.Id).Result == false);
            }

        }
    }
}
