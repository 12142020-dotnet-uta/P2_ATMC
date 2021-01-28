using Microsoft.EntityFrameworkCore;
using SpaceBook.Business;
using SpaceBook.Models;
using SpaceBook.Models.ViewModels;
using SpaceBook.Repository;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SpaceBook.Tests
{
    public class PictureTests
    {
        private const string API_KEY = "api_key=71wMNdfgtD6jcpVBtmULKEqxPjbhhXLSit3mQqXu";
        private const string APOD_NASA_API = "https://api.nasa.gov/planetary/apod?";

        [Fact]
        public void CheckAddToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            //create a picture to add
            Picture testPicture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.ImageURL.com/thisimage",
                MediaType = MediaType.image,
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
                Assert.True(repo.AttemptAddPictureToDb(testPicture).Result);


            }
            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                var tempPicture = repo.GetPictureById(testPicture.PictureID).Result;

                //test that all properties were successfully saved

                Assert.Equal(testPicture.ImageURL, tempPicture.ImageURL);
                Assert.Equal(testPicture.Description, tempPicture.Description);
                Assert.Equal(testPicture.MediaType, tempPicture.MediaType);
                Assert.Equal(testPicture.Title, tempPicture.Title);
                Assert.Equal(testPicture.Date, tempPicture.Date);
                context.Database.EnsureDeleted();
            }
        }


        [Fact]
        public void CheckAddToDatabaseFomAPOD_API()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            Mapper mapper = new Mapper();
            List<Picture> pictures = new List<Picture>();

            string strQueryDate = $"start_date={DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd")}&end_date={DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}&";
            //we get the picture of the day.
            using (HttpClient httpClient = new HttpClient())
            {
                string pictureJSON = "";
                Task<string> responseBodyTask = httpClient.GetStringAsync(APOD_NASA_API + strQueryDate + API_KEY);
                responseBodyTask.Wait();
                pictureJSON = responseBodyTask.Result;
                foreach (APOD_PictureViewModel pictureVM in JsonSerializer.Deserialize<IEnumerable<APOD_PictureViewModel>>(pictureJSON))
                {
                    Picture picture = mapper.ConvertAPOD_PictureViewModelIntoPictureModel(pictureVM);
                    pictures.Add(picture);
                }
            }



            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //test add picture to db
                foreach (Picture testPicture in pictures)
                {
                    Assert.True(repo.AttemptAddPictureToDb(testPicture).Result);
                }


            }
            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                var pictureList = repo.GetAllPictures().Result;

                //test that all properties were successfully saved

                var picturesDB = pictureList as ICollection<Picture>;
                Assert.Equal(3, picturesDB.Count);
                context.Database.EnsureDeleted();
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
                ImageURL = "www.ImageURL.com/thisimage",
                MediaType = MediaType.image,
                Title = "The Test Picture"
            };
            Picture testPicture2 = new Picture()
            {
                Date = DateTime.Now,
                Description = "something else descriptive",
                ImageURL = "www.ImageURL.com/thatimage",
                MediaType = MediaType.image,
                Title = "Another Test Picture"
            };
            Picture testPicture3 = new Picture()
            {
                Date = DateTime.Now,
                Description = "something else descriptive",
                ImageURL = "www.ImageURL.com/notanimage",
                MediaType = MediaType.video,
                Title = "The Test video"
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //test add picture to db
                Assert.True(repo.AttemptAddPictureToDb(testPicture).Result);
                Assert.True(repo.AttemptAddPictureToDb(testPicture2).Result);
                Assert.True(repo.AttemptAddPictureToDb(testPicture3).Result);


            }
            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                var pictureList = repo.GetAllPictures().Result;

                //test that all properties were successfully saved

                var pictures = pictureList as ICollection<Picture>;
                Assert.Equal(3,pictures.Count);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void CheckGetAllFromAPI()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            //create a picture to add
            Picture testPicture = new Picture()
            {
                Date = DateTime.Now,
                Description = "something descriptive",
                ImageURL = "www.ImageURL.com/thisimage",
                MediaType = MediaType.image,
                Title = "The Test Picture"
            };
            Picture testPicture2 = new Picture()
            {
                Date = DateTime.Now,
                Description = "something else descriptive",
                ImageURL = "www.ImageURL.com/thatimage",
                MediaType = MediaType.image,
                Title = "Another Test Picture"
            };
            Picture testPicture3 = new Picture()
            {
                Date = DateTime.Now,
                Description = "something else descriptive",
                ImageURL = "www.ImageURL.com/notanimage",
                MediaType = MediaType.video,
                Title = "The Test video"
            };

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //test add picture to db
                Assert.True(repo.AttemptAddPictureToDb(testPicture).Result);
                Assert.True(repo.AttemptAddPictureToDb(testPicture2).Result);
                Assert.True(repo.AttemptAddPictureToDb(testPicture3).Result);


            }
            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                var pictureList = repo.GetAllPictures().Result;

                //test that all properties were successfully saved

                var pictures = pictureList as ICollection<Picture>;
                Assert.Equal(3, pictures.Count);
                context.Database.EnsureDeleted();
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
                ImageURL = "www.ImageURL.com/thisimage",
                MediaType = MediaType.image,
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
                Assert.True(repo.AttemptAddPictureToDb(testPicture).Result);


            }
            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //make sure picture is in db
                Assert.True(repo.IsPictureInDb(testPicture.PictureID).Result);
                //remove picture from db
                Assert.True(repo.AttemptRemovePictureFromDb(testPicture.PictureID).Result);
                //make sure picture is not in db
                Assert.False(repo.IsPictureInDb(testPicture.PictureID).Result);
                context.Database.EnsureDeleted();
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
                ImageURL = "www.ImageURL.com/thisimage",
                MediaType = MediaType.image,
                Title = "The Test Picture"
            };

            Picture editedPicture = new Picture()
            {
                PictureID = testPicture.PictureID,
                Date = DateTime.Now,
                Description = "something even more descriptive",
                ImageURL = "www.ImageURL.com/thatimage",
                MediaType = MediaType.image,
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
                Assert.True(repo.AttemptAddPictureToDb(testPicture).Result);

            }
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                //test add picture to db
                Picture temp = repo.GetPictureById(testPicture.PictureID).Result;
                temp.Date = editedPicture.Date;
                temp.Description = editedPicture.Description;
                temp.ImageURL = editedPicture.ImageURL;
                temp.MediaType = editedPicture.MediaType;
                temp.Title = editedPicture.Title;
                //attempt edit
                Assert.True(repo.AttemptEditPictureInDb(temp).Result);
            }

            //try to access db with a new instance of a repository
            using (var context = new ApplicationDbContext(options))
            {
                //create repository layer
                PictureRepository repo = new PictureRepository(context);

                var tempPicture = repo.GetPictureById(testPicture.PictureID).Result;

                //test that all properties were successfully saved

                Assert.Equal(editedPicture.ImageURL, tempPicture.ImageURL);
                Assert.Equal(editedPicture.Description, tempPicture.Description);
                Assert.Equal(editedPicture.MediaType, tempPicture.MediaType);
                Assert.Equal(editedPicture.Title, tempPicture.Title);
                Assert.Equal(editedPicture.Date, tempPicture.Date);

                context.Database.EnsureDeleted();
            }
        }
    }
}
