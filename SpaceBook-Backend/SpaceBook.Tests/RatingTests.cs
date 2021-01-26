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
                RatingRepository ratingRepo = new RatingRepository(context);

                testRating.RatedPicture = testPicture;
                testRating.RatedPictureId = testPicture.PictureID;
                testRating.UserRating = user;
                testRating.UserRatingId = user.Id;
                testRating.Value = 4.5d;

                //AttemptAddRating returns true if adding to db is successful
                Assert.True(ratingRepo.AttemptAddRating(testRating).Result);
            }

            using (var context = new ApplicationDbContext(options))
            {
                RatingRepository ratingRepo = new RatingRepository(context);

                //Use IsRatingInDB to check that the rating was added to the DB
                Assert.True(ratingRepo.IsRatingInDb(testRating.RatingID).Result);
                context.Database.EnsureDeleted();
            }


        }

        [Fact]
        public void CheckGetRatingFromDatabaseById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb1").Options;
           
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

                bool test = ratingRepo.AttemptAddRating(testRating).Result;
                
           
                //Check to see that the rating retrieved by ID is the same as the rating that was added
                Assert.Equal(testRating, ratingRepo.GetRatingById(testRating.RatingID).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckGetRatingsFromDatabaseByUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb2").Options;
            
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

                bool test = ratingRepo.AttemptAddRating(testRating).Result;

                //Create list and add the test rating to it
                List<Rating> ratings = new List<Rating>();
                ratings.Add(testRating);

                //Check to see that the List of ratings you created is the same as the list retrieved from the db
                Assert.Equal(ratings, ratingRepo.GetRatingsByUser(user.Id).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckGetRatingsFromDatabaseByPicture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb3").Options;
            
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

                bool test = ratingRepo.AttemptAddRating(testRating).Result;

                List<Rating> ratings = new List<Rating>();
                ratings.Add(testRating);

                //Check to see that the List of ratings you created is the same as the list retrieved from the db using the picture ID
                Assert.Equal(ratings, ratingRepo.GetRatingsForPicture(testPicture.PictureID).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckGetRatingFromDatabaseByUserAndPicture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb4").Options;
            
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
                          
                bool test = ratingRepo.AttemptAddRating(testRating).Result;

                //Check to see that the rating you added is the same as the rating retrieved from the db using the picture ID and user ID
                Assert.Equal(testRating, ratingRepo.GetRatingByPictureAndUser(testPicture.PictureID, user.Id).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckEditRatingInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb5").Options;
            
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

                bool test = ratingRepo.AttemptAddRating(testRating).Result;

                Assert.Equal(testRating, ratingRepo.GetRatingById(testRating.RatingID).Result);


                testRating.Value = 3.7d;
                //Attempts to edit the rating in the DB and returns true if successful
                Assert.True(ratingRepo.AttemptEditRating(testRating).Result);

                //Get the rating in the DB by the ID and verify the Value property was changed from 4.5 to 3.7
                Assert.True(ratingRepo.GetRatingById(testRating.RatingID).Result.Value == 3.7);
            }

        }

        [Fact]
        public void CheckDeleteRatingInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb6").Options;
            
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

                bool test = ratingRepo.AttemptAddRating(testRating).Result;

                //Show that the rating was added
                Assert.True(ratingRepo.IsRatingInDb(testRating.RatingID).Result);

                //Attempts to delete the rating in the DB and returns true if successful
                Assert.True(ratingRepo.AttemptRemoveRating(testRating.RatingID).Result);

                //verify the rating is no longer in the db
                Assert.True(ratingRepo.IsRatingInDb(testRating.RatingID).Result == false);
            }

        }


    }
}
