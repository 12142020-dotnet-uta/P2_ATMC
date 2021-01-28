using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using SpaceBook.Repository;
using Xunit;

namespace SpaceBook.Tests
{
    public class CommentTests
    {

        [Fact]
        public void CheckCommentAddToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "MyDb123").Options;


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


            Comment comment = new Comment();
            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                CommentRepository commentRepo = new CommentRepository(context);

                comment.CommentText = "this is a comment";
                comment.Date = DateTime.Now;
                comment.UserCommented = user;
                comment.UserCommentedId = user.Id;
                comment.PictureCommented = picture;
                comment.PictureCommentedId = picture.PictureID;
                comment.ParentComment = null;
                comment.ParentCommentId = null;

                //bool will return true if the comment was added
                bool test = commentRepo.AttemptAddCommentToDb(comment).Result;

                //check to see if the comment made it to the DB
                Assert.True(commentRepo.IsCommentInDb(comment.CommentID).Result);
                context.Database.EnsureDeleted();


            }

        }

        [Fact]
        public void CheckGetCommentfromDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "200").Options;


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


            Comment comment = new Comment();
            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                CommentRepository commentRepo = new CommentRepository(context);

                comment.CommentText = "this is a comment";
                comment.Date = DateTime.Now;
                comment.UserCommented = user;
                comment.UserCommentedId = user.Id;
                comment.PictureCommented = picture;
                comment.PictureCommentedId = picture.PictureID;
                comment.ParentComment = null;
                comment.ParentCommentId = null;

                //bool will return true if the comment was added
                bool test = commentRepo.AttemptAddCommentToDb(comment).Result;

                //Check to see that if the comment and the one in the DB is the same
                Assert.Equal(comment, commentRepo.GetCommentById(comment.CommentID).Result);
                context.Database.EnsureDeleted();


            }

        }

        [Fact]
        public void CheckGetAllCommentsForParentComment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "201").Options;


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


            Comment comment = new Comment();
            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                CommentRepository commentRepo = new CommentRepository(context);

                comment.CommentText = "this is a comment";
                comment.Date = DateTime.Now;
                comment.UserCommented = user;
                comment.UserCommentedId = user.Id;
                comment.PictureCommented = picture;
                comment.PictureCommentedId = picture.PictureID;
                comment.ParentComment = null;
                comment.ParentCommentId = null;

                Comment comment2 = new Comment();
                comment2.CommentText = "this is a comment";
                comment2.Date = DateTime.Now;
                comment2.UserCommented = user;
                comment2.UserCommentedId = user.Id;
                comment2.PictureCommented = picture;
                comment2.PictureCommentedId = picture.PictureID;
                comment2.ParentComment = comment;
                comment2.ParentCommentId = comment.CommentID;




                //bool will return true if the comment was added
                bool test = commentRepo.AttemptAddCommentToDb(comment).Result;

                //bool to see if second comment is added to DB
                bool test2 = commentRepo.AttemptAddCommentToDb(comment2).Result;

                //check to see if parent comment matches the one in DB
                Assert.Equal(comment2.CommentID, commentRepo.GetAllCommentsForParentComment(comment.CommentID).Result.FirstOrDefault().CommentID);
                context.Database.EnsureDeleted();


            }
        }

        [Fact]
        public void CheckGetAllCommentsByUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "202").Options;


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


            Comment comment = new Comment();
            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                CommentRepository commentRepo = new CommentRepository(context);

                comment.CommentText = "this is a comment";
                comment.Date = DateTime.Now;
                comment.UserCommented = user;
                comment.UserCommentedId = user.Id;
                comment.PictureCommented = picture;
                comment.PictureCommentedId = picture.PictureID;
                comment.ParentComment = null;
                comment.ParentCommentId = null;

                //bool will return true if the comment was added
                bool test = commentRepo.AttemptAddCommentToDb(comment).Result;

                //check to see if comments below to a specific user
                Assert.Equal(user.Id, commentRepo.GetAllCommentsByUser(user.Id).Result.FirstOrDefault().UserCommentedId);
                context.Database.EnsureDeleted();


            }
        }

        [Fact]
        public void CheckGetAllCommentsForPicture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "202").Options;


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


            Comment comment = new Comment();
            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                CommentRepository commentRepo = new CommentRepository(context);

                comment.CommentText = "this is a comment";
                comment.Date = DateTime.Now;
                comment.UserCommented = user;
                comment.UserCommentedId = user.Id;
                comment.PictureCommented = picture;
                comment.PictureCommentedId = picture.PictureID;
                comment.ParentComment = null;
                comment.ParentCommentId = null;

                //bool will return true if the comment was added
                bool test = commentRepo.AttemptAddCommentToDb(comment).Result;

                //check to see if comments come from a picture
                Assert.Equal(comment.CommentID, commentRepo.GetAllCommentsForPicture(picture.PictureID).Result.FirstOrDefault().CommentID);
                context.Database.EnsureDeleted();


            }
        }

        [Fact]
        public void CheckAttemptEditComment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "203").Options;


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


            Comment comment = new Comment();
            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                CommentRepository commentRepo = new CommentRepository(context);

                comment.CommentText = "this is a comment";
                comment.Date = DateTime.Now;
                comment.UserCommented = user;
                comment.UserCommentedId = user.Id;
                comment.PictureCommented = picture;
                comment.PictureCommentedId = picture.PictureID;
                comment.ParentComment = null;
                comment.ParentCommentId = null;

                //bool will return true if the comment was added
                bool test = commentRepo.AttemptAddCommentToDb(comment).Result;

                //change the comment
                comment.CommentText = "this is an edit";


                //check to see if comments below to a specific user
                Assert.True(commentRepo.AttemptEditComment(comment).Result);
                context.Database.EnsureDeleted();


            }
        }

        [Fact]
        public void CheckAttemptRemoveComment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "204").Options;


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


            Comment comment = new Comment();
            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                CommentRepository commentRepo = new CommentRepository(context);

                comment.CommentText = "this is a comment";
                comment.Date = DateTime.Now;
                comment.UserCommented = user;
                comment.UserCommentedId = user.Id;
                comment.PictureCommented = picture;
                comment.PictureCommentedId = picture.PictureID;
                comment.ParentComment = null;
                comment.ParentCommentId = null;

                //bool will return true if the comment was added
                bool test = commentRepo.AttemptAddCommentToDb(comment).Result;

            


                //check to see if comments below to a specific user
                Assert.True(commentRepo.AttemptRemoveComment(comment.CommentID).Result);

                //verify that the comment is no longer in the DB
                Assert.True(commentRepo.IsCommentInDb(comment.CommentID).Result == false);



            }
        }



    }
}