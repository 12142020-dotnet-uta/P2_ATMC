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
        public UserPicture GetUserPictureById(int userPictureId)
        {
            return _dbContext.UserPictures.Include(x => x.Picture).Include(x => x.UploadedBy).FirstOrDefault(x => x.UserPictureID == userPictureId);
        }

        /// <summary>
        /// Returns a list of all user pictures in the Db
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserPicture> GetAllUserPictures()
        {
            return _dbContext.UserPictures.Include(x=>x.Picture).Include(x=>x.UploadedBy);
        }

        /// <summary>
        /// Returns true if the given user picture Id exists in the db, and false if not
        /// </summary>
        /// <param name="userPictureId"></param>
        /// <returns></returns>
        public bool IsUserPictureInDb(int userPictureId)
        {
            //attempt to get picture from the db
            UserPicture userPicture = GetUserPictureById(userPictureId);
            //return true if the picture is not null
            return userPicture != null;
        }

        /// <summary>
        /// Attempts to add a new user picture to the db; Returns false if the user picture already exists in the db or it is not added to the db
        /// </summary>
        /// <param name="userPicture"></param>
        /// <returns></returns>
        public bool AttemptAddUserPictureToDb(UserPicture userPicture)
        {
            if (IsUserPictureInDb(userPicture.UserPictureID))
            {
                //fail if picture is already in db
                return false;
            }
            //attempt to add picture to db
            _dbContext.UserPictures.Add(userPicture);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Attempts to edit the user picture in the db with the same id as the given picture; returns false if user picture is not in db already
        /// </summary>
        /// <param name="userPicture"></param>
        /// <returns></returns>
        public bool AttemptEditUserPictureInDb(UserPicture userPicture)
        {
            if (!IsUserPictureInDb(userPicture.UserPictureID))
            {
                //fail if picture is not in db
                return false;
            }
            //make changes and save
            _dbContext.UserPictures.Update(userPicture);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Attempts to remove picture from db; returns false if user picture was not in db beforehand; returns false if user picture is still in db after attempting to remove it.
        /// </summary>
        /// <param name="userPictureId"></param>
        /// <returns></returns>
        public bool AttemptRemovePictureFromDb(int userPictureId)
        {
            if (!IsUserPictureInDb(userPictureId))
            {
                return false;
            }
            //get pictcure to remove
            UserPicture userPicture = GetUserPictureById(userPictureId);
            //attempt to remove
            _dbContext.UserPictures.Remove(userPicture);
            _dbContext.SaveChanges();
            //return true if picture is no longer in db
            return !IsUserPictureInDb(userPictureId);
        }

    }

}
