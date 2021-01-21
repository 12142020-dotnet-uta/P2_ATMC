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
        public Picture GetPictureById(int pictureId)
        {
            return _dbContext.Pictures.FirstOrDefault(x => x.PictureID == pictureId);
        }

        /// <summary>
        /// Returns a list of all pictures in the Db
        /// </summary>
        /// <returns></returns>
        public List<Picture> GetAllPictures()
        {
            return _dbContext.Pictures.ToList();
        }

        /// <summary>
        /// Returns true if the given picture Id exists in the db, and false if not
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public bool IsPictureInDb(int pictureId)
        {
            //attempt to get picture from the db
            Picture picture = GetPictureById(pictureId);
            //return true if the picture is not null
            return picture != null;
        }

        /// <summary>
        /// Attempts to add a new picture to the db; Returns false if the picture already exists in the db or it is not added to the db
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public bool AttemptAddPictureToDb(Picture picture)
        {
            if (IsPictureInDb(picture.PictureID))
            {
                //fail if picture is already in db
                return false;
            }
            //attempt to add picture to db
            _dbContext.Pictures.Add(picture);
            _dbContext.SaveChanges();
            if (IsPictureInDb(picture.PictureID))
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
        public bool AttemptEditPictureInDb(Picture picture)
        {
            if (!IsPictureInDb(picture.PictureID)) 
            {
                //fail if picture is not in db
                return false;
            }
            //make changes and save
            Picture original = GetPictureById(picture.PictureID);
            original.MediaType = picture.MediaType;
            original.ImageURL = picture.ImageURL;
            original.Title = picture.Title;
            original.Description = picture.Description;
            original.Date = picture.Date;
            _dbContext.SaveChanges();
            //check that changes were made
            Picture checking = GetPictureById(picture.PictureID);
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
        public bool AttemptRemovePictureFromDb(int pictureId)
        {
            if (!IsPictureInDb(pictureId))
            { 
                return false; 
            }
            if (IsPictureUserPicture(pictureId))
            {
                //TODO: remove user picture first
            }
            //get pictcure to remove
            Picture picture = GetPictureById(pictureId);
            //attempt to remove
            _dbContext.Pictures.Remove(picture);
            _dbContext.SaveChanges();
            return IsPictureInDb(pictureId);
        }

        /// <summary>
        /// Returns whether or not the given picture id is a picture created by a user
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public bool IsPictureUserPicture(int pictureId)
        {
            //attempt to get the user picture that references this picture
            UserPicture userPicture = _dbContext.UserPictures.FirstOrDefault(x => x.PictureId == pictureId);
            //picture is userpicture if that ^ is not null
            return userPicture != null;

        }

        /// <summary>
        /// Returns a list of all of the pictures saved by the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Picture> GetAllFavoritePicturesForUser(string userId)
        {
            return _dbContext.Favorites.Include(x => x.Picture).Where(x => x.UserId == userId).Select(x => x.Picture).ToList();
        }

    }

}
