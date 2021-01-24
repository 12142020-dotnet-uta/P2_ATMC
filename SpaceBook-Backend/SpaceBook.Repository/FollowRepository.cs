using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Repository
{
    public class FollowRepository
    {
        private ApplicationDbContext _dbContext;

        public FollowRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns the Follow from the database with the given ID. Returns null if none exist.
        /// </summary>
        /// <param name="followId"></param>
        /// <returns></returns>
        public Follow GetFollowById(int followId)
        {
            return _dbContext.Follows.Include(x => x.Follower).Include(x => x.Followed).FirstOrDefault(x => x.FollowID == followId);
        }

        public Follow GetFollowByFollowerAndFollowedIds(string followerId, string followedId)
        {
            return _dbContext.Follows.Include(x => x.Follower).Include(x => x.Followed).FirstOrDefault(x => x.FollowerId == followerId && x.FollowedId == followedId);
        }

        /// <summary>
        /// Returns all Follows from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Follow> GetAllFollow()
        {
            return _dbContext.Follows.Include(x => x.Follower).Include(x => x.Followed);
        }

        /// <summary>
        /// Returns all followers of the ApplicationUser with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Follow> GetFollowersOfUser(string userId)
        {
            return _dbContext.Follows.Include(x=>x.Follower).Where(x => x.FollowedId == userId);
        }
        
        /// <summary>
        /// Returns all Follows of the ApplicationUser with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Follow> GetFollowedOfUser(string userId)
        {
            return _dbContext.Follows.Include(x=>x.Followed).Where(x => x.FollowerId == userId);
        }

        /// <summary>
        /// Checks to see if a Follow with the given ID is in the database.
        /// Returns true if it is, false if it isn't.
        /// </summary>
        /// <param name="followId"></param>
        /// <returns></returns>
        public bool IsFollowInDb(int followId)
        {
            return GetFollowById(followId) != null;
        }

        /// <summary>
        /// Attempts to add a Follow to the database.
        /// Checks to see if a Follow with same ID as the given Follow already exists in the database.
        /// Returns false if it does
        /// Adds the Follow to the database and returns true if it doesn't.
        /// </summary>
        /// <param name="follow"></param>
        /// <returns></returns>
        public bool AttemptAddFollow(Follow follow)
        {
            if (IsFollowInDb(follow.FollowID))
            {
                return false;
            }
            else
            {
                _dbContext.Follows.Add(follow);
                _dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Attempts to remove a Follow from the database.
        /// Checks to see if a Follow with same ID as the given Follow already exists in the database.
        /// Returns false if it doesn't,
        /// Removes the Follow from the database and returns true if it doesn't.
        /// </summary>
        /// <param name="follow"></param>
        /// <returns></returns>
        public bool AttemptRemoveFollow(int followId)
        {
            Follow follow = GetFollowById(followId);
            if (follow == null)
            {
                return false;
            }
            else
            {
                _dbContext.Follows.Remove(follow);
                _dbContext.SaveChanges();
                return true;
            }
        }
    }
}
