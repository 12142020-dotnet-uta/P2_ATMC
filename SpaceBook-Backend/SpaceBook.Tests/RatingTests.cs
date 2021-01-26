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
    public class RatingTests
    {
        [Fact]
        public void CheckRatingAddToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;
            //var context = new ApplicationDbContext(options);
            //context.Database.EnsureDeleted();
            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };
            
            Picture testPicture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };
            Rating testRating = new Rating();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                PictureRepository pictureRepo = new PictureRepository(context);
                ApplicationUserRepository userRepo = new ApplicationUserRepository(context);
                RatingRepository ratingRepo = new RatingRepository(context);

                testRating.RatedPicture = testPicture;
                testRating.RatedPictureId = testPicture.PictureID;
                testRating.UserRating = user;
                testRating.UserRatingId = user.Id;
                testRating.Value = 4.5d;

                Assert.True(ratingRepo.AttemptAddRating(testRating).Result);
            }

            using (var context = new ApplicationDbContext(options))
            {
                RatingRepository ratingRepo = new RatingRepository(context);
                Assert.True(ratingRepo.IsRatingInDb(testRating.RatingID).Result);
                context.Database.EnsureDeleted();
            }


        }

        [Fact]
        public void CheckGetRatingFromDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;
            //var context = new ApplicationDbContext(options);
            //context.Database.EnsureDeleted();
            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            Picture testPicture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };
            Rating testRating = new Rating();

            using (var context = new ApplicationDbContext(options))
            {

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                RatingRepository ratingRepo = new RatingRepository(context);

                testRating.RatedPicture = testPicture;
                testRating.RatedPictureId = testPicture.PictureID;
                testRating.UserRating = user;
                testRating.UserRatingId = user.Id;
                testRating.Value = 4.5d;

                context.Users.Add(user);
                context.Pictures.Add(testPicture);
                context.SaveChanges();
                bool test = ratingRepo.AttemptAddRating(testRating).Result;
                
           

                Assert.Equal(testRating, ratingRepo.GetRatingById(testRating.RatingID).Result);
                context.Database.EnsureDeleted();
            }

        }


    }
}
