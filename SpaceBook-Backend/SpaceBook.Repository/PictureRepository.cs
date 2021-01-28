using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Repository
{
    public class PictureRepository
    {
        private ApplicationDbContext _dbContext;

        public PictureRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns a picture based on the given picture Id; returns null if the picture is not in the db;
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<Picture> GetPictureById(int pictureId)
        {
            return await _dbContext.Pictures.AsQueryable().FirstOrDefaultAsync(x => x.PictureID == pictureId);
        }

        /// <summary>
        /// This method have the optional parameters PageNumber and PageSize to the default value, if the PageNumber is default, it returns the Complete List of Pictures, else it returns
        /// with pagination the elements in the DB.
        /// </summary>
        /// <returns>Returns a async Enumerable of Pictures.</returns>
        public async Task<IEnumerable<Picture>> GetAllPictures(int PageNumber = 1, int PageSize = 20)
        {
            //If Default Value, return All Pictures
            //if (PageNumber == 1)
            //{
                return await _dbContext.Pictures.ToListAsync();
            //}
            //else
            //{
            //    return await _dbContext.Pictures
            //        .Skip( (PageNumber -1 ) * PageSize  )
            //        .Take(PageSize).ToListAsync();

            //}
        }

        /// <summary>
        /// Returns true if the given picture Id exists in the db, and false if not
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<bool> IsPictureInDb(int pictureId)
        {
            //attempt to get picture from the db
            Picture picture = await GetPictureById(pictureId);
            //return true if the picture is not null
            return picture != null;
        }

        /// <summary>
        /// Attempts to add a new picture to the db; Returns false if the picture already exists in the db or it is not added to the db
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public async Task<bool> AttemptAddPictureToDb(Picture picture)
        {
            if (await IsPictureInDb(picture.PictureID) )
            {
                //fail if picture is already in db
                return false;
            }
            //attempt to add picture to db
            _dbContext.Pictures.Add(picture);
            await _dbContext.SaveChangesAsync();
            if (await IsPictureInDb(picture.PictureID))
            {
                //success if picture is now in db
                return true;
            }
            //something must have gone wrong
            return false;
        }

        /// <summary>
        /// Attempts to edit the picture in the db with the same id as the given picture; returns false if picture is not in db already, or if changes aren't successfully added to the db
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public async Task<bool> AttemptEditPictureInDb(Picture picture)
        {
            if (!(await IsPictureInDb(picture.PictureID))) 
            {
                //fail if picture is not in db
                return false;
            }
            //make changes and save
            _dbContext.Pictures.Update(picture);
            await _dbContext.SaveChangesAsync();
            //check that changes were made
            Picture checking =  await GetPictureById(picture.PictureID);
            if(checking.MediaType == picture.MediaType &&
                checking.ImageURL == picture.ImageURL &&
                checking.Title == picture.Title &&
                checking.Description == picture.Description &&
                checking.Date == picture.Date)
            {
                //if all values are correct return true
                return true;
            }
            //editing was unsuccessful for some reason
            return false;
        }

        /// <summary>
        /// Attempts to remove picture from db; returns false if picture was not in db beforehand; returns false if picture is still in db after attempting to remove it.
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<bool> AttemptRemovePictureFromDb(int pictureId)
        {
            if (!(await IsPictureInDb(pictureId)))
            { 
                return false; 
            }
            if (await IsPictureUserPicture(pictureId))
            {
                //TODO: remove user picture first
            }
            //get pictcure to remove
            Picture picture = await GetPictureById(pictureId);
            //attempt to remove
            _dbContext.Pictures.Remove(picture);
            await _dbContext.SaveChangesAsync();
            //return true if picture is no longer in db
            return !(await IsPictureInDb(pictureId));
        }

        /// <summary>
        /// Returns whether or not the given picture id is a picture created by a user
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<bool> IsPictureUserPicture(int pictureId)
        {
            //attempt to get the user picture that references this picture
            UserPicture userPicture = await _dbContext.UserPictures.AsQueryable().FirstOrDefaultAsync(x => x.PictureId == pictureId);
            //picture is userpicture if that ^ is not null
            return userPicture != null;

        }

        /// <summary>
        /// Returns a list of all of the pictures saved by the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Picture>> GetAllFavoritePicturesForUser(string userId)
        {
            return await _dbContext.Favorites.Include(x => x.Picture).Where(x => x.UserId == userId).Select(x => x.Picture).ToListAsync();
        }

        /// <summary>
        /// Search in the DB if exists the photo of the day, if exists, it returns the Picture, otherwise, it returns null
        /// </summary>
        /// <returns>Returns a async Task of Picture if exists in the DB, else is null.</returns>
        public async Task<Picture> IsPictureOfTheDayInDBAsync()
        {
            //Picture picture= await _dbContext.Pictures.AsQueryable().FirstOrDefaultAsync( x => x.Date.ToString() == DateTime.Now.ToString("dd-MM-yy") + " 00:00:00");
            Picture picture = await _dbContext.Pictures
                .Where(picture => picture.isUserPicture == false )
                .OrderByDescending(picture => picture.Date).Take(5)
                .FirstOrDefaultAsync();
            try
            {
                if (picture.Date.ToString("dd-MM-yy") == DateTime.Now.ToString("dd-MM-yy"))
                    return picture;
                else
                    return null;
            }
            catch {return null;}

        }
    }

}
