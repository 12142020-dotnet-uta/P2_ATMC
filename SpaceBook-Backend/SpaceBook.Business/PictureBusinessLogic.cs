﻿using SpaceBook.Models;
using SpaceBook.Repository;
using System;
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

        public async Task<Comment> CreateCommentOnPicture(int pictureId, string username, int? parentCommentId, string commentText)
        {
            ApplicationUser user = await _userRepository.GetUserByUsername(username);
            Picture picture = await _pictureRepository.GetPictureById(pictureId);
            Comment parentComment = null;
            if (parentCommentId != null)
            {
                if (await _commentRepository.IsCommentInDb((int)parentCommentId))
                {
                    parentComment = await _commentRepository.GetCommentById((int)parentCommentId);
                }
                else
                {
                    //trying to comment on a comment that doesn't exist
                    return null;
                }
            }

            Comment comment = new Comment()
            {
                CommentText = commentText,
                UserCommented = user,
                UserCommentedId = user.Id,
                PictureCommented = picture,
                PictureCommentedId = picture.PictureID,
                ParentComment = parentComment,
                ParentCommentId = parentCommentId
            };
            if (await _commentRepository.AttemptAddCommentToDb(comment))
            {
                return comment;
            }
            else return null;
        }
        /// <summary>
        /// Gets all comments for the given picture id
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetCommentsForPicture(int pictureId)
        {
            return await _commentRepository.GetAllCommentsForPicture(pictureId);
        }
        /// <summary>
        /// Gets all child comments for the given comment id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetCommentsForComment(int commentId)
        {
            return await _commentRepository.GetAllCommentsForParentComment(commentId);
        }
        public async Task<bool> DeleteComment(int commentId, string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var comment = await _commentRepository.GetCommentById(commentId);
            if (user == null || comment == null)
            {
                return false;
            }

            var childComments =await _commentRepository.GetAllCommentsForParentComment(commentId);
            foreach(Comment child in childComments)
            {
                child.ParentComment = comment.ParentComment;
                child.ParentCommentId = comment.ParentCommentId;
                await _commentRepository.AttemptEditComment(child);
            }
            return await _commentRepository.AttemptRemoveComment(commentId);
        }

        public async Task<Comment> EditComment(int commentId,string username, string commentText)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var comment = await _commentRepository.GetCommentById(commentId);
            if (user == null || comment == null)
            {
                return null;
            }

            comment.CommentText = commentText;
            //update time?
            if (await _commentRepository.AttemptEditComment(comment))
            {
                return comment;
            }
            else return null;
        }


        #endregion
    }
}
