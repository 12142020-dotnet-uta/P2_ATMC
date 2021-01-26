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
    public class FavoriteTests
    {
        [Fact]
        public void CheckFavoriteAddToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "1").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            Picture picture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };
            Favorite favorite = new Favorite();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                FavoriteRepository favoriteRepo = new FavoriteRepository(context);

                favorite.Picture = picture;
                favorite.PictureId = picture.PictureID;
                favorite.User = user;
                favorite.UserId = user.Id;
          

                //AttemptAddFavorite returns true if adding to db is successful
                Assert.True(favoriteRepo.AttemptAddFavorite(favorite).Result);
            }

            using (var context = new ApplicationDbContext(options))
            {
                FavoriteRepository favoriteRepo = new FavoriteRepository(context);

                //Use IsFavoriteInDB to check that the rating was added to the DB
                Assert.True(favoriteRepo.IsFavoriteInDb(favorite.FavoriteID).Result);
                context.Database.EnsureDeleted();
            }


        }

        [Fact]
        public void CheckDeleteFavoriteInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "2").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            Picture picture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };
            Favorite favorite = new Favorite();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                FavoriteRepository favoriteRepo = new FavoriteRepository(context);

                favorite.Picture = picture;
                favorite.PictureId = picture.PictureID;
                favorite.User = user;
                favorite.UserId = user.Id;


                //AttemptAddFavorite returns true if adding to db is successful
                bool test = favoriteRepo.AttemptAddFavorite(favorite).Result;
            

                //Show that the favorite was added
                Assert.True(favoriteRepo.IsFavoriteInDb(favorite.FavoriteID).Result);


                //Attempts to delete the favorite in the DB and returns true if successful
                Assert.True(favoriteRepo.AttemptRemoveFavorite(favorite.FavoriteID).Result);

                //verify the favorite is no longer in the db
                Assert.True(favoriteRepo.IsFavoriteInDb(favorite.FavoriteID).Result == false);
            }

        }

        [Fact]
        public void CheckGetFavoriteFromDatabaseById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "3").Options;
            
            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            Picture picture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };
            Favorite favorite = new Favorite();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                FavoriteRepository favoriteRepo = new FavoriteRepository(context);

                favorite.Picture = picture;
                favorite.PictureId = picture.PictureID;
                favorite.User = user;
                favorite.UserId = user.Id;


                //AttemptAddFavorite returns true if adding to db is successful
                Assert.True(favoriteRepo.AttemptAddFavorite(favorite).Result);

                //Make sure that the favorite retrieved from DB is the same as the favorite added
                Assert.Equal(favorite, favoriteRepo.GetFavoriteById(favorite.FavoriteID).Result);
            }

        }

        [Fact]
        public void CheckGetFavoritesFromDatabaseByUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "4").Options;
            
            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            Picture picture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };
            Favorite favorite = new Favorite();

            using (var context = new ApplicationDbContext(options))
            {

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                FavoriteRepository favoriteRepo = new FavoriteRepository(context);

                favorite.Picture = picture;
                favorite.PictureId = picture.PictureID;
                favorite.User = user;
                favorite.UserId = user.Id;

                bool test = favoriteRepo.AttemptAddFavorite(favorite).Result;

                //Create list and add the test favorite to it
                List<Favorite> favorites = new List<Favorite>();
                favorites.Add(favorite);

                //Check to see that the List of favorites you created is the same as the list retrieved from the db
                Assert.Equal(favorites, favoriteRepo.GetFavoritesByUser(user.Id).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckGetFavoritesFromDatabaseByPicture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "5").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            Picture picture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };
            Favorite favorite = new Favorite();

            using (var context = new ApplicationDbContext(options))
            {

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                FavoriteRepository favoriteRepo = new FavoriteRepository(context);

                favorite.Picture = picture;
                favorite.PictureId = picture.PictureID;
                favorite.User = user;
                favorite.UserId = user.Id;

                bool test = favoriteRepo.AttemptAddFavorite(favorite).Result;

                //Create list and add the test favorite to it
                List<Favorite> favorites = new List<Favorite>();
                favorites.Add(favorite);

                //Check to see that the List of favorites you created is the same as the list retrieved from the db
                Assert.Equal(favorites, favoriteRepo.GetFavoritesByPicture(picture.PictureID).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckGetFavoriteFromDatabaseByUserAndPicture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "6").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            Picture picture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };
            Favorite favorite = new Favorite();

            using (var context = new ApplicationDbContext(options))
            {

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                FavoriteRepository favoriteRepo = new FavoriteRepository(context);

                favorite.Picture = picture;
                favorite.PictureId = picture.PictureID;
                favorite.User = user;
                favorite.UserId = user.Id;

                bool test = favoriteRepo.AttemptAddFavorite(favorite).Result;


                //Check to see that the favorite you created is the same as the favorite retrieved from the db with picture id and user id
                Assert.Equal(favorite, favoriteRepo.GetFavoriteByUserPicture(user.Id, picture.PictureID).Result);
                context.Database.EnsureDeleted();
            }

        }
    }
}
