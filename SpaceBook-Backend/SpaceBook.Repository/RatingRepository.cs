using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Repository
{
    public class RatingRepository
    {
        public ApplicationDbContext _dbContext;

        public RatingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns the Rating with the given Rating ID.
        /// </summary>
        /// <param name="ratingId"></param>
        /// <returns></returns>
        public Rating GetRatingById(int ratingId)
        {
            return _dbContext.Ratings.Include(x=>x.RatedPicture).Include(x=>x.UserRating).FirstOrDefault(x => x.RatingID == ratingId);
        }

        /// <summary>
        /// Returns all of the Ratings in the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rating> GetAllRatings()
        {
            return _dbContext.Ratings.Include(x => x.RatedPicture).Include(x => x.UserRating);
        }

        /// <summary>
        /// Returns all of the Ratings in the database that have an ApplicationUser with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Rating> GetRatingsByUser(string userId)
        {
            return _dbContext.Ratings.Include(x => x.RatedPicture).Where(x => x.UserRatingId == userId);
        }

        /// <summary>
        /// Returns all of the Ratings in the database that have a Picture with the given picture ID.
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public IEnumerable<Rating> GetRatingsForPicture(int pictureId)
        {
            return _dbContext.Ratings.Include(x => x.UserRating).Where(x => x.RatedPictureId == pictureId);
        }

        /// <summary>
        /// Checks if a Rating with the given ID is in the database.
        /// Returns true if it is, false if it isn't.
        /// </summary>
        /// <param name="ratingId"></param>
        /// <returns></returns>
        public bool IsRatingInDb(int ratingId)
        {
            return GetRatingById(ratingId) != null;
        }

        /// <summary>
        /// Checks if there is a Rating in the database that has the same ID as the given Rating.
        /// If there is, return false
        /// If there isn't, adds a new Rating to the database and returns true
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        public bool AttemptAddRating(Rating rating)
        {
            if (IsRatingInDb(rating.RatingID))
            {
                return false;
            }
            else
            {
                _dbContext.Ratings.Add(rating);
                _dbContext.SaveChanges();
                return true;
            }

        }

        /// <summary>
        /// Checks if there is a Rating in the database that has the same ID as the given Rating.
        /// If there isn't, return false
        /// If there is, updates the Rating in the database and returns true
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        public bool AttemptEditRating(Rating rating)
        {
            if (!IsRatingInDb(rating.RatingID))
            {
                return false;
            }
            else
            {
                _dbContext.Ratings.Update(rating);
                _dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Checks if there is a Rating in the database that has the same ID as the given Rating.
        /// If there isn't, return false
        /// If there is, remove the Rating from the database and returns true
        /// </summary>
        /// <param name="ratingId"></param>
        /// <returns></returns>
        public bool AttemptRemoveRating(int ratingId)
        {
            Rating rating = GetRatingById(ratingId);
            if (rating == null)
            {
                return false;
            }
            else
            {
                _dbContext.Ratings.Remove(rating);
                _dbContext.SaveChanges();
                return true;
            }
        }

    }
}
