using System;
using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using SpaceBook.Repository;
using Xunit;

namespace SpaceBook.Tests
{
    public class UserPictureTests
    {
        

            [Fact]
            public void CheckAddUserPictureToDb()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "401").Options;

                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "testuser",
                    Email = "test@test.com"
                };

                Picture picture = new Picture()
                {
                    Date = DateTime.Now,
                    Description = "something descriptive",
                    ImageURL = "www.imageurl.com/thisimage",
                    MediaType = MediaType.image,
                    Title = "The Test Picture"
                };

                UserPicture userPicture = new UserPicture();


                using (var context = new ApplicationDbContext(options))
                {
                    //clean in memory db
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    UserPictureRepository userPictureRepository = new UserPictureRepository(context);


                    userPicture.UploadedBy = user;
                    userPicture.UploadedById = user.Id;
                    userPicture.Picture = picture;
                    userPicture.PictureId = picture.PictureID;

                    //bool will return true if the user picture was added
                    bool test = userPictureRepository.AttemptAddUserPictureToDb(userPicture).Result;

                    //check to see if the user picture made it to the DB
                    Assert.True(userPictureRepository.IsUserPictureInDb(userPicture.PictureId).Result);
                    context.Database.EnsureDeleted();
                }
            }

        [Fact]
        public void CheckGetUserPictureByPicture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "402").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com"
            };

            Picture picture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.image,
                Title = "The Test Picture"
            };

            UserPicture userPicture = new UserPicture();


            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                UserPictureRepository userPictureRepository = new UserPictureRepository(context);


                userPicture.UploadedBy = user;
                userPicture.UploadedById = user.Id;
                userPicture.Picture = picture;
                userPicture.PictureId = picture.PictureID;

                //bool will return true if the user picture was added
                bool test = userPictureRepository.AttemptAddUserPictureToDb(userPicture).Result;

                //check to retrieve if we can get the picture
                Assert.Equal(userPicture.UploadedById,userPictureRepository.GetUserPictureByPicture(picture).Result.UploadedById);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void CheckAttemptRemoveUserPictureFromDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "403").Options;


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
                MediaType = MediaType.image,
                Title = "The Test Picture"
            };


            UserPicture userPicture = new UserPicture();
            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                UserPictureRepository userPictureRepository = new UserPictureRepository(context);


                userPicture.UploadedBy = user;
                userPicture.UploadedById = user.Id;
                userPicture.Picture = picture;
                userPicture.PictureId = picture.PictureID;


                //bool will return true if the user picture was added
                bool test = userPictureRepository.AttemptAddUserPictureToDb(userPicture).Result;




                //check to see if user photo was removed from DB
                Assert.True(userPictureRepository.AttemptRemoveUserPictureFromDb(picture.PictureID).Result);

                //verify that the user photo is no longer in the DB
                Assert.True(userPictureRepository.IsUserPictureInDb(userPicture.PictureId).Result == false);



            }
        }

        [Fact]
        public void CheckAttemptEditUserPictureInDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "404").Options;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com"
            };

            Picture picture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.image,
                Title = "The Test Picture"
            };

            Picture picture2 = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thatimage",
                MediaType = MediaType.image,
                Title = "The Test Picture"
            };



            UserPicture userPicture = new UserPicture();


            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                UserPictureRepository userPictureRepository = new UserPictureRepository(context);


                userPicture.UploadedBy = user;
                userPicture.UploadedById = user.Id;
                userPicture.Picture = picture;
                userPicture.PictureId = picture.PictureID;

               

                //bool will return true if the user picture was added
                bool test = userPictureRepository.AttemptAddUserPictureToDb(userPicture).Result;


                userPicture.Picture = picture2;

                //check to see if the user picture edit made it to the DB

                Assert.True(userPictureRepository.AttemptEditUserPictureInDb(userPicture).Result == true);

                Assert.Equal(userPicture, userPictureRepository.GetUserPictureById(userPicture.UserPictureID).Result);
                context.Database.EnsureDeleted();

            }
        }






    }
}
