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
    public class FollowTests
    {
        [Fact]
        public void CheckFollowAddToDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "7").Options;

            ApplicationUser follower = new ApplicationUser()
            {
                FirstName = "Follower",
                LastName = "User",
                UserName = "followeruser",
                Email = "test@test.com",
            };

            ApplicationUser followed = new ApplicationUser()
            {
                FirstName = "Followed",
                LastName = "User",
                UserName = "followeduser",
                Email = "test2@test2.com",
            };
            Follow follow = new Follow();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                FollowRepository followRepo = new FollowRepository(context);

                follow.Followed = followed;
                follow.FollowedId = followed.Id;
                follow.Follower = follower;
                follow.FollowerId = follower.Id;

                //AttemptAddfollow returns true if adding to db is successful
                Assert.True(followRepo.AttemptAddFollow(follow).Result);
            }

            using (var context = new ApplicationDbContext(options))
            {
                FollowRepository followRepo = new FollowRepository(context);

                //Use IsfollowInDB to check that the rating was added to the DB
                Assert.True(followRepo.IsFollowInDb(follow.FollowID).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckDeleteFollowInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "8").Options;

            ApplicationUser follower = new ApplicationUser()
            {
                FirstName = "Follower",
                LastName = "User",
                UserName = "followeruser",
                Email = "test@test.com",
            };

            ApplicationUser followed = new ApplicationUser()
            {
                FirstName = "Followed",
                LastName = "User",
                UserName = "followeduser",
                Email = "test2@test2.com",
            };
            Follow follow = new Follow();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                FollowRepository followRepo = new FollowRepository(context);

                follow.Followed = followed;
                follow.FollowedId = followed.Id;
                follow.Follower = follower;
                follow.FollowerId = follower.Id;


                //AttemptAddfollow returns true if adding to db is successful
                bool test = followRepo.AttemptAddFollow(follow).Result;


                //Show that the follow was added
                Assert.True(followRepo.IsFollowInDb(follow.FollowID).Result);


                //Attempts to delete the follow in the DB and returns true if successful
                Assert.True(followRepo.AttemptRemoveFollow(follow.FollowID).Result);

                //verify the follow is no longer in the db
                Assert.True(followRepo.IsFollowInDb(follow.FollowID).Result == false);
            }

        }

        [Fact]
        public void CheckGetFollowFromDatabaseById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "9").Options;

            ApplicationUser follower = new ApplicationUser()
            {
                FirstName = "Follower",
                LastName = "User",
                UserName = "followeruser",
                Email = "test@test.com",
            };

            ApplicationUser followed = new ApplicationUser()
            {
                FirstName = "Followed",
                LastName = "User",
                UserName = "followeduser",
                Email = "test2@test2.com",
            };
            Follow follow = new Follow();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                FollowRepository followRepo = new FollowRepository(context);

                follow.Followed = followed;
                follow.FollowedId = followed.Id;
                follow.Follower = follower;
                follow.FollowerId = follower.Id;


                //AttemptAddFollow returns true if adding to db is successful
                bool test = followRepo.AttemptAddFollow(follow).Result;

                //Make sure that the Follow retrieved from DB is the same as the favorite added
                Assert.Equal(follow, followRepo.GetFollowById(follow.FollowID).Result);
            }

        }

        [Fact]
        public void CheckGetFollowsFromDatabaseWhereUserIsFollower()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "10").Options;

            ApplicationUser follower = new ApplicationUser()
            {
                FirstName = "Follower",
                LastName = "User",
                UserName = "followeruser",
                Email = "test@test.com",
            };

            ApplicationUser followed = new ApplicationUser()
            {
                FirstName = "Followed",
                LastName = "User",
                UserName = "followeduser",
                Email = "test2@test2.com",
            };
            Follow follow = new Follow();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                FollowRepository followRepo = new FollowRepository(context);

                follow.Followed = followed;
                follow.FollowedId = followed.Id;
                follow.Follower = follower;
                follow.FollowerId = follower.Id;

                bool test = followRepo.AttemptAddFollow(follow).Result;

                //Create list and add the first follow to it
                List<Follow> follows = new List<Follow>();
                follows.Add(follow);

                //Check to see that the List of follow you created is the same as the list retrieved from the db 
                Assert.Equal(follows, followRepo.GetFollowedOfUser(follower.Id).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckGetFollowsFromDatabaseWhereUserIsFollowed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "11").Options;

            ApplicationUser follower = new ApplicationUser()
            {
                FirstName = "Follower",
                LastName = "User",
                UserName = "followeruser",
                Email = "test@test.com",
            };

            ApplicationUser followed = new ApplicationUser()
            {
                FirstName = "Followed",
                LastName = "User",
                UserName = "followeduser",
                Email = "test2@test2.com",
            };
            Follow follow = new Follow();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                FollowRepository followRepo = new FollowRepository(context);

                follow.Followed = followed;
                follow.FollowedId = followed.Id;
                follow.Follower = follower;
                follow.FollowerId = follower.Id;

                bool test = followRepo.AttemptAddFollow(follow).Result;

                //Create list and add the first follow to it
                List<Follow> follows = new List<Follow>();
                follows.Add(follow);

                //Check to see that the List of follow you created is the same as the list retrieved from the db 
                Assert.Equal(follows, followRepo.GetFollowersOfUser(followed.Id).Result);
                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public void CheckGetFollowFromDatabaseByFollowerAndFollowed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "12").Options;

            ApplicationUser follower = new ApplicationUser()
            {
                FirstName = "Follower",
                LastName = "User",
                UserName = "followeruser",
                Email = "test@test.com",
            };

            ApplicationUser followed = new ApplicationUser()
            {
                FirstName = "Followed",
                LastName = "User",
                UserName = "followeduser",
                Email = "test2@test2.com",
            };
            Follow follow = new Follow();

            using (var context = new ApplicationDbContext(options))
            {
                //clean in memory db
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //create repository layer
                FollowRepository followRepo = new FollowRepository(context);

                follow.Followed = followed;
                follow.FollowedId = followed.Id;
                follow.Follower = follower;
                follow.FollowerId = follower.Id;

                bool test1 = followRepo.AttemptAddFollow(follow).Result;



                //Check to see that the follow you created is the same as the follow retrieved from the db with follower and followed ids
                Assert.Equal(follow, followRepo.GetFollowByFollowerAndFollowedIds(follower.Id, followed.Id).Result);
                context.Database.EnsureDeleted();
            }

        }
    }
}
