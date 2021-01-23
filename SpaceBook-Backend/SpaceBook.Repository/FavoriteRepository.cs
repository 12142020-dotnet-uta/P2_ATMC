using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Repository
{
    public class FavoriteRepository
    {
        public ApplicationDbContext _dbContext;

        public FavoriteRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns the Favorite from the database with the given ID. Returns null if none exist.
        /// </summary>
        /// <param name="favoriteId"></param>
        /// <returns></returns>
        public Favorite GetFavoriteById(int favoriteId)
        {
            return _dbContext.Favorites.Include(x=>x.Picture).Include(x=>x.User).FirstOrDefault(x => x.FavoriteID == favoriteId);
        }

        /// <summary>
        /// Returns all Favorites from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Favorite> GetAllFavorites()
        {
            return _dbContext.Favorites.Include(x => x.Picture).Include(x => x.User);
        }

        /// <summary>
        /// Returns all Favorites of a specific ApplicationUser in the database given the specified user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Favorite> GetFavoritesByUser(string userId)
        {
            return _dbContext.Favorites.Include(x=>x.Picture).Where(x => x.UserId == userId);
        }

        /// <summary>
        /// Returns all Favorites of a specific Picture in the database given the specified picture ID.
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public IEnumerable<Favorite> GetFavoritesByPicture(int pictureId)
        {
            return _dbContext.Favorites.Include(x => x.User).Where(x => x.PictureId == pictureId);
        }

        /// <summary>
        /// Checks to see if a Favorite with the given ID is in the database.
        /// Returns true if it is, false if it isn't.
        /// </summary>
        /// <param name="favoriteId"></param>
        /// <returns></returns>
        public bool IsFavoriteInDb(int favoriteId)
        {
            return GetFavoriteById(favoriteId) != null;
        }

        /// <summary>
        /// Attempts to add a Favorite to the database.
        /// Checks to see if a Favorite with same ID as the given Favorite already exists in the database.
        /// Returns false if it does
        /// Adds the Favorite to the database and returns true if it doesn't.
        /// </summary>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public bool AttemptAddFavorite(Favorite favorite)
        {
            if (IsFavoriteInDb(favorite.FavoriteID))
            {
                return false;
            }
            else
            {
                _dbContext.Favorites.Add(favorite);
                _dbContext.SaveChanges();
                return true;
            }
        }


        /// <summary>
        /// Attempts to remove a Favorite from the database.
        /// Checks to see if a Favorite with same ID as the given Favorite already exists in the database.
        /// Returns false if it doesn't,
        /// Removes the Favorite from the database and returns true if it doesn't.
        /// </summary>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public bool AttemptRemoveFavorite(int favoriteId)
        {
            Favorite favorite = GetFavoriteById(favoriteId);
            if (favorite == null)
            {
                return false;
            }
            else
            {
                _dbContext.Favorites.Remove(favorite);
                _dbContext.SaveChanges();
                return true;
            }
        }


    }
}
