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
        public async Task<Rating> GetRatingById(int ratingId)
        {
            return await _dbContext.Ratings.Include(x=>x.RatedPicture).Include(x=>x.UserRating).AsQueryable().FirstOrDefaultAsync<Rating>(x => x.RatingID == ratingId);
        }

        /// <summary>
        /// Returns the Rating based on the ids of the user and picture passed in
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Rating> GetRatingByPictureAndUser(int pictureId, string userId)
        {
            var userRatings = await GetRatingsByUser(userId);

            return await userRatings.Where(x => x.RatedPictureId == pictureId).AsQueryable().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns all of the Ratings in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Rating>> GetAllRatings()
        {
            return await _dbContext.Ratings.Include(x => x.RatedPicture).Include(x => x.UserRating).AsQueryable().ToListAsync<Rating>();
        }

        /// <summary>
        /// Returns all of the Ratings in the database that have an ApplicationUser with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Rating>> GetRatingsByUser(string userId)
        {
            return await _dbContext.Ratings.Include(x => x.RatedPicture).Where(x => x.UserRatingId == userId).AsQueryable().ToListAsync<Rating>();
        }

        /// <summary>
        /// Returns all of the Ratings in the database that have a Picture with the given picture ID.
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Rating>> GetRatingsForPicture(int pictureId)
        {
            return await _dbContext.Ratings.Include(x => x.UserRating).Where(x => x.RatedPictureId == pictureId).AsQueryable().ToListAsync<Rating>();
        }

        /// <summary>
        /// Checks if a Rating with the given ID is in the database.
        /// Returns true if it is, false if it isn't.
        /// </summary>
        /// <param name="ratingId"></param>
        /// <returns></returns>
        public async Task<bool> IsRatingInDb(int ratingId)
        {
            return await GetRatingById(ratingId) != null;
        }

        /// <summary>
        /// Checks if there is a Rating in the database that has the same ID as the given Rating.
        /// If there is, return false
        /// If there isn't, adds a new Rating to the database and returns true
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        public async Task<bool> AttemptAddRating(Rating rating)
        {
            if (await IsRatingInDb(rating.RatingID))
            {
                return false;
            }
            else
            {
                _dbContext.Ratings.Add(rating);
                await _dbContext.SaveChangesAsync();
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
        public async Task<bool> AttemptEditRating(Rating rating)
        {
            Rating rtg = await GetRatingById(rating.RatingID);
            if (rtg == null)
            {
                return false;
            }
            else
            {
                _dbContext.Ratings.Update(rating);
                await _dbContext.SaveChangesAsync();
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
        public async Task<bool> AttemptRemoveRating(int ratingId)
        {
            Rating rating = await GetRatingById(ratingId);
            if (rating == null)
            {
                return false;
            }
            else
            {
                _dbContext.Ratings.Remove(rating);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

    }
}
