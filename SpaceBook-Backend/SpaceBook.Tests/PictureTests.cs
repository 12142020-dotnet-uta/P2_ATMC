using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using SpaceBook.Repository;
using System;
using Xunit;

namespace SpaceBook.Tests
{
    public class PictureTests
    {
        
        [Fact]
        public void CheckAddToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            //create a picture to add
            Picture testPicture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //test add picture to db
                Assert.True( repo.AttemptAddPictureToDb(testPicture));


            }
            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                var tempPicture = repo.GetPictureById(testPicture.PictureID);

                //test that all properties were successfully saved

                Assert.Equal(testPicture.ImageURL, tempPicture.ImageURL);
                Assert.Equal(testPicture.Description, tempPicture.Description);
                Assert.Equal(testPicture.MediaType, tempPicture.MediaType);
                Assert.Equal(testPicture.Title, tempPicture.Title);
                Assert.Equal(testPicture.Date, tempPicture.Date);

            }
        }
        [Fact]
        public void CheckGetAllFromDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            //create a picture to add
            Picture testPicture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };
            Picture testPicture2 = new Picture()
            {
                Date = DateTime.Now,
                Description = "something else descriptive",
                ImageURL = "www.imageurl.com/thatimage",
                MediaType = MediaType.Image,
                Title = "Another Test Picture"
            };
            Picture testPicture3 = new Picture()
            {
                Date = DateTime.Now,
                Description = "something else descriptive",
                ImageURL = "www.imageurl.com/notanimage",
                MediaType = MediaType.Video,
                Title = "The Test Video"
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //test add picture to db
                Assert.True(repo.AttemptAddPictureToDb(testPicture));
                Assert.True(repo.AttemptAddPictureToDb(testPicture2));
                Assert.True(repo.AttemptAddPictureToDb(testPicture3));


            }
            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                var pictureList = repo.GetAllPictures();

                //test that all properties were successfully saved

                Assert.Equal(3,pictureList.Count);

            }
        }
        [Fact]
        public void CheckRemoveFromDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            //create a picture to add
            Picture testPicture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //test add picture to db
                Assert.True(repo.AttemptAddPictureToDb(testPicture));


            }
            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //make sure picture is in db
                Assert.True(repo.IsPictureInDb(testPicture.PictureID));
                //remove picture from db
                Assert.True(repo.AttemptRemovePictureFromDb(testPicture.PictureID));
                //make sure picture is not in db
                Assert.False(repo.IsPictureInDb(testPicture.PictureID));

            }
        }
        [Fact]
        public void CheckEditInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            //create a picture to add
            Picture testPicture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.imageurl.com/thisimage",
                MediaType = MediaType.Image,
                Title = "The Test Picture"
            };

            Picture editedPicture = new Picture()
            {
                PictureID = testPicture.PictureID,
                Date = DateTime.Now,
                Description = "something even more descriptive",
                ImageURL = "www.imageurl.com/thatimage",
                MediaType = MediaType.Image,
                Title = "The New Test Picture"
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //attempt add picture to db
                Assert.True(repo.AttemptAddPictureToDb(testPicture));

            }
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //test add picture to db
                Picture temp = repo.GetPictureById(testPicture.PictureID);
                temp.Date = editedPicture.Date;
                temp.Description = editedPicture.Description;
                temp.ImageURL = editedPicture.ImageURL;
                temp.MediaType = editedPicture.MediaType;
                temp.Title = editedPicture.Title;
                //attempt edit
                Assert.True(repo.AttemptEditPictureInDb(temp));
            }

            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                var tempPicture = repo.GetPictureById(testPicture.PictureID);

                //test that all properties were successfully saved

                Assert.Equal(editedPicture.ImageURL, tempPicture.ImageURL);
                Assert.Equal(editedPicture.Description, tempPicture.Description);
                Assert.Equal(editedPicture.MediaType, tempPicture.MediaType);
                Assert.Equal(editedPicture.Title, tempPicture.Title);
                Assert.Equal(editedPicture.Date, tempPicture.Date);

            }
        }
    }
}
