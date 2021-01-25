using SpaceBook.Models;
using SpaceBook.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceBook.Business
{
    public class PictureBusinessLogic
    {
        private readonly PictureRepository _pictureRepository;
        private readonly UserPictureRepository _userPictureRepository;
        private readonly ApplicationUserRepository _userRepository;
        private readonly RatingRepository _ratingRepository;
        private readonly CommentRepository _commentRepository;
        public PictureBusinessLogic(PictureRepository pictureRepository, ApplicationUserRepository userRepository,UserPictureRepository userPictureRepository, RatingRepository ratingRepository,CommentRepository commentRepository )
        {
            _pictureRepository = pictureRepository;
            _userRepository = userRepository;
            _userPictureRepository = userPictureRepository;
            _ratingRepository = ratingRepository;
            _commentRepository = commentRepository;
        }

        public async Task<Picture> GetPicture(int pictureId)
        {
            return await _pictureRepository.GetPictureById(pictureId);
        }

        public async Task<IEnumerable<Picture>> GetAllPictures()
        {
            return await _pictureRepository.GetAllPictures();
        }
        #region User Pictures
        public async Task<bool> CreateUserPicture(Picture picture, string username)
        {
            var user =  await _userRepository.GetUserByUsername(username);

            UserPicture userPicture = new UserPicture()
            {
                Picture = picture,
                PictureId = picture.PictureID,
                UploadedById = user.Id,
                UploadedBy = user,
            };

            //do checks on picture before adding?
            //like make sure user is logged in?
            return await _userPictureRepository.AttemptAddUserPictureToDb(userPicture);
        }

        /// <summary>
        /// Takes a picture Id, and attmepts to delete both the user picture(relationship model) and the picture(object model) from the db
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserPicture(int pictureId, string username)
        {
            var userPicture = await _userPictureRepository.GetUserPictureByPicture(await _pictureRepository.GetPictureById(pictureId));
            if(userPicture.UploadedBy.UserName != username)
            {
                //users can only delete their own pictures.
                return false;
            }
            //delete userpicture
            if (await _userPictureRepository.AttemptRemoveUserPictureFromDb(userPicture.UserPictureID))
            {
                //delete comments and ratings on picture
                foreach (Comment comment in await _commentRepository.GetAllCommentsForPicture(userPicture.PictureId))
                {
                    await _commentRepository.AttemptRemoveComment(comment.CommentID);
                }
                foreach (Rating rating in await _ratingRepository.GetRatingsForPicture(userPicture.PictureId))
                {
                    await _ratingRepository.AttemptRemoveRating(rating.RatingID);
                }
                //if user picture is deleted, delete picture and return whether or not it was successful
                return await _pictureRepository.AttemptRemovePictureFromDb(userPicture.PictureId);
            }
            else
            {
                //the attempt to remove the user picture failed
                return false;
            }
        }
        #endregion

        #region Ratings
        /// <summary>
        /// Takes the username of the user creating the rating, the id of the picture being rated, and the value of the rating, and creates a new rating
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pictureId"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        public async Task<bool> CreateRating(string username, int pictureId, double rating)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var picture = await _pictureRepository.GetPictureById(pictureId);

            //var user = await userTask;
            //var picture = await pictureTask;

            if (user == null || picture == null)
            {
                return false;
            }
            Rating newRating = new Rating()
            {
                Value = rating,
                RatedPicture = picture,
                RatedPictureId = picture.PictureID,
                UserRating = user,
                UserRatingId = user.Id
            };
            return await _ratingRepository.AttemptAddRating(newRating);
        }
        /// <summary>
        /// edits a rating in the db based on the username, pictureId, and new value for the rating
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pictureId"></param>
        /// <param name="newRating"></param>
        /// <returns></returns>
        public async Task<bool> EditRating(string username, int pictureId, double newRating)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null) { 
                //invalid user
                return false; 
            }
            var retrievedRating = await _ratingRepository.GetRatingByPictureAndUser(pictureId, user.Id);
            if (retrievedRating == null)
            {
                //no rating found
                return false;
            }
            retrievedRating.Value = newRating;
            return await _ratingRepository.AttemptEditRating(retrievedRating); 
        }

        public async Task<Rating> GetRating(string userId, int pictureId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                //invalid user
                return null;
            }
            return await _ratingRepository.GetRatingByPictureAndUser(pictureId,user.Id);
        }

        public async Task<IEnumerable<Rating>> GetRatingsForPicture(int pictureId)
        {
            return await _ratingRepository.GetRatingsForPicture(pictureId);
        }

        #endregion

        #region Comments


        #endregion
    }
}
