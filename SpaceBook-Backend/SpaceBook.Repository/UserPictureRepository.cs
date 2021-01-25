using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Repository
{
    public class UserPictureRepository
    {
        private ApplicationDbContext _dbContext;

        public UserPictureRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns a user picture based on the given user picture Id; returns null if the user picture is not in the db;
        /// </summary>
        /// <param name="userPictureId"></param>
        /// <returns></returns>
        public async Task<UserPicture> GetUserPictureById(int userPictureId)
        {
            return await _dbContext.UserPictures.Include(x => x.Picture).Include(x => x.UploadedBy).AsQueryable().FirstOrDefaultAsync(x => x.UserPictureID == userPictureId);
        }
        /// <summary>
        /// Returns a user picture (relationship model) based on the given picture Id(actual picture); returns null if the user picture is not in the db;
        /// </summary>
        /// <param name="userPictureId"></param>
        /// <returns></returns>
        public async Task<UserPicture> GetUserPictureByPicture(Picture picture)
        {
            return await _dbContext.UserPictures.Include(x => x.Picture).Include(x => x.UploadedBy).AsQueryable().FirstOrDefaultAsync(x => x.PictureId == picture.PictureID);
        }

        /// <summary>
        /// Returns a list of all user pictures in the Db
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserPicture>> GetAllUserPictures()
        {
            return await _dbContext.UserPictures.Include(x=>x.Picture).Include(x=>x.UploadedBy).ToListAsync();
        }

        /// <summary>
        /// Returns true if the given user picture Id exists in the db, and false if not
        /// </summary>
        /// <param name="userPictureId"></param>
        /// <returns></returns>
        public async Task<bool> IsUserPictureInDb(int userPictureId)
        {
            //attempt to get picture from the db
            UserPicture userPicture = await GetUserPictureById(userPictureId);
            //return true if the picture is not null
            return userPicture != null;
        }

        /// <summary>
        /// Attempts to add a new user picture to the db; Returns false if the user picture already exists in the db or it is not added to the db
        /// </summary>
        /// <param name="userPicture"></param>
        /// <returns></returns>
        public async Task<bool> AttemptAddUserPictureToDb(UserPicture userPicture)
        {
            if (await IsUserPictureInDb(userPicture.UserPictureID))
            {
                //fail if picture is already in db
                return false;
            }
            //attempt to add picture to db
            _dbContext.UserPictures.Add(userPicture);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Attempts to edit the user picture in the db with the same id as the given picture; returns false if user picture is not in db already
        /// </summary>
        /// <param name="userPicture"></param>
        /// <returns></returns>
        public async Task<bool> AttemptEditUserPictureInDb(UserPicture userPicture)
        {
            if (!(await IsUserPictureInDb(userPicture.UserPictureID)))
            {
                //fail if picture is not in db
                return false;
            }
            //make changes and save
            _dbContext.UserPictures.Update(userPicture);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Attempts to remove user picture from db (this doesn't delete the actual picture, only the relationship model between a user and a picture); returns false if user picture was not in db beforehand; returns false if user picture is still in db after attempting to remove it.
        /// </summary>
        /// <param name="userPictureId"></param>
        /// <returns></returns>
        public async Task<bool> AttemptRemoveUserPictureFromDb(int userPictureId)
        {
            if (!(await IsUserPictureInDb(userPictureId)))
            {
                //failed to delete if the picture is not in the db
                return false;
            }
            //get pictcure to remove
            UserPicture userPicture = await GetUserPictureById(userPictureId);
            //attempt to remove
            _dbContext.UserPictures.Remove(userPicture);
            await _dbContext.SaveChangesAsync();
            //return true if picture is no longer in db
            return !(await IsUserPictureInDb(userPictureId));
        }

    }

}
