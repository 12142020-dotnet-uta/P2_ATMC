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
        public async Task<Favorite> GetFavoriteById(int favoriteId)
        {
            return await _dbContext.Favorites.Include(x=>x.Picture).Include(x=>x.User).AsQueryable().FirstOrDefaultAsync<Favorite>(x => x.FavoriteID == favoriteId);
        }

        public async Task<Favorite> GetFavoriteByUserPicture(string userId, int pictureId)
        {
            return await _dbContext.Favorites.Include(x => x.Picture).Include(x => x.User).AsQueryable().FirstOrDefaultAsync<Favorite>(x => x.UserId == userId && x.PictureId == pictureId);
        }

        /// <summary>
        /// Returns all Favorites from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Favorite>> GetAllFavorites()
        {
            return await _dbContext.Favorites.Include(x => x.Picture).Include(x => x.User).ToListAsync<Favorite>();
        }

        /// <summary>
        /// Returns all Favorites of a specific ApplicationUser in the database given the specified user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Picture>> GetFavoritesByUser(string userId)
        {
            IEnumerable<Favorite> favorites = await _dbContext.Favorites.Include(x=>x.Picture).AsQueryable().Where(x => x.UserId == userId).ToListAsync<Favorite>();
            List<Picture> favoritedPictures = new List<Picture>();
            foreach(Favorite f in favorites)
            {
                favoritedPictures.Add(f.Picture);
            }
            return favoritedPictures;
        }

        /// <summary>
        /// Returns all Favorites of a specific Picture in the database given the specified picture ID.
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Favorite>> GetFavoritesByPicture(int pictureId)
        {
            return await _dbContext.Favorites.Include(x => x.User).AsQueryable().Where(x => x.PictureId == pictureId).ToListAsync<Favorite>();
        }

        /// <summary>
        /// Checks to see if a Favorite with the given ID is in the database.
        /// Returns true if it is, false if it isn't.
        /// </summary>
        /// <param name="favoriteId"></param>
        /// <returns></returns>
        public async Task<bool> IsFavoriteInDb(int favoriteId)
        {
            return await GetFavoriteById(favoriteId) != null;
        }

        /// <summary>
        /// Attempts to add a Favorite to the database.
        /// Checks to see if a Favorite with same ID as the given Favorite already exists in the database.
        /// Returns false if it does
        /// Adds the Favorite to the database and returns true if it doesn't.
        /// </summary>
        /// <param name="favorite"></param>
        /// <returns></returns>
        public async Task<bool> AttemptAddFavorite(Favorite favorite)
        {
            if (await IsFavoriteInDb(favorite.FavoriteID) || await GetFavoriteByUserPicture(favorite.UserId, favorite.PictureId) != null)
            {
                return false;
            }
            else
            {
                _dbContext.Favorites.Add(favorite);
                await _dbContext.SaveChangesAsync();
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
        public async Task<bool> AttemptRemoveFavorite(int favoriteId)
        {
            Favorite favorite = await GetFavoriteById(favoriteId);
            if (favorite == null)
            {
                return false;
            }
            else
            {
                _dbContext.Favorites.Remove(favorite);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }


    }
}
