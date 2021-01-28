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
    public class MessageTests
    {
        [Fact]
        public void CheckAttemptAddMessage()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "301").Options;


            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };


            Message message = new Message();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                MessageRepository messageRepo = new MessageRepository(context);

                message.Text = "this is some text";
                message.Date = DateTime.Now;
                message.Sender = user;
                message.SenderId = user.Id;
                message.ParentMessage = null;



                //bool will return true if the comment was added
                bool test = messageRepo.AttemptAddMessage(message).Result;

                //check to see if the comment made it to the DB
                Assert.True(messageRepo.IsMessageInDb(message.MessageID).Result);
                context.Database.EnsureDeleted();


            }
        }


        [Fact]
        public void CheckAttemptRemoveMessage()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "302").Options;


            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };


            Message message = new Message();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                MessageRepository messageRepo = new MessageRepository(context);

                message.Text = "this is some text";
                message.Date = DateTime.Now;
                message.Sender = user;
                message.SenderId = user.Id;
                message.ParentMessage = null;



                //bool will return true if the message was added
                bool test = messageRepo.AttemptAddMessage(message).Result;

                //check to see if message is removed
                Assert.True(messageRepo.AttemptRemoveMessage(message.MessageID).Result);

                //verify that the message is no longer in the DB
                Assert.True(messageRepo.IsMessageInDb(message.MessageID).Result == false);


            }
        }


        [Fact]
        public void CheckGetMessageById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "303").Options;


            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };


            Message message = new Message();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                MessageRepository messageRepo = new MessageRepository(context);

                message.Text = "this is some text";
                message.Date = DateTime.Now;
                message.Sender = user;
                message.SenderId = user.Id;
                message.ParentMessage = null;



                //bool will return true if the message was added
                bool test = messageRepo.AttemptAddMessage(message).Result;

                //Check to see that if the message and the one in the DB is the same
                Assert.Equal(message, messageRepo.GetMessageById(message.MessageID).Result);
                context.Database.EnsureDeleted();


            }
        }

        [Fact]
        public void CheckGetMessagesByUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "304").Options;


            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            ApplicationUser user2 = new ApplicationUser()
            {
                FirstName = "Test2",
                LastName = "Test2",
                UserName = "testuser2",
                Email = "test2@test.com",
            };

            Message message = new Message();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                MessageRepository messageRepo = new MessageRepository(context);

                message.Text = "this is some text";
                message.Date = DateTime.Now;
                message.Sender = user;
                message.SenderId = user.Id;
                message.Recipient = user2;
                message.RecipientId = user2.Id;
                message.ParentMessage = null;

                List<Message> messagesTest = new List<Message>();
                messagesTest.Add(message);


                //bool will return true if the message was added
                bool test = messageRepo.AttemptAddMessage(message).Result;


                //check to see if message below to a specific user
                Assert.Equal(messagesTest, messageRepo.GetMessagesByUser(user.Id).Result);
                context.Database.EnsureDeleted();



            }
        }

        [Fact]
        public void CheckGetSentMessagesByUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "305").Options;


            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

  

            Message message = new Message();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                MessageRepository messageRepo = new MessageRepository(context);

                message.Text = "this is some text";
                message.Date = DateTime.Now;
                message.Sender = user;
                message.SenderId = user.Id;
                message.ParentMessage = null;

  


                //bool will return true if the message was added
                bool test = messageRepo.AttemptAddMessage(message).Result;


                //check to see if message below to a specific user
                Assert.Equal(user.Id, messageRepo.GetSentMessagesByUser(user.Id).Result.FirstOrDefault().SenderId);
                context.Database.EnsureDeleted();
               

            }
        }

        [Fact]
        public void CheckGetReceivedMessagesByUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "306").Options;


            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Test",
                LastName = "Test",
                UserName = "testuser",
                Email = "test@test.com",
            };

            ApplicationUser user2 = new ApplicationUser()
            {
                FirstName = "Test2",
                LastName = "Test2",
                UserName = "testuser2",
                Email = "test2@test.com",
            };



            Message message = new Message();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                MessageRepository messageRepo = new MessageRepository(context);

                message.Text = "this is some text";
                message.Date = DateTime.Now;
                message.Sender = user;
                message.SenderId = user.Id;
                message.Recipient = user2;
                message.RecipientId = user2.Id;
                message.ParentMessage = null;



                //bool will return true if the message was added
                bool test = messageRepo.AttemptAddMessage(message).Result;


                //check to see if a specific messages below belong to a specific recipient
                Assert.Equal(user2.Id, messageRepo.GetReceivedMessagesByUser(user2.Id).Result.FirstOrDefault().RecipientId);
                context.Database.EnsureDeleted();


            }
        }











    }
}