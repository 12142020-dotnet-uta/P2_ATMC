using SpaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Repository
{
    public class CommentRepository
    {
        private ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            dbContext = _dbContext;
        }
        

        /// <summary>
        /// Returns a comment given the specified Comment ID. Returns null if no comment has that ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Comment GetCommentById(int commentId)
        {
            return _dbContext.Comments.FirstOrDefault(x => x.CommentID == commentId);
        }

        /// <summary>
        /// Returns all comments in the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllComments()
        {
            return _dbContext.Comments;
        }

        /// <summary>
        /// Returns true if the given comment ID is in the Database. Otherwise returns false.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsCommentInDb(int id)
        {
            return GetCommentById(id) != null;
        }

        /// <summary>
        /// Attempts to Add Comment to db. 
        /// Checks to see if comment ID is already in Db. 
        /// Returns false if it is. Adds the Comment to the DB and returns true if it isn't.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool AttemptAddCommentToDb(Comment comment)
        {
            if (IsCommentInDb(comment.CommentID))
            {
                return false;
            }
            else
            {
                _dbContext.Comments.Add(comment);
                _dbContext.SaveChanges();
                return true;
            }
            
        }

        /// <summary>
        /// Attempts to edit Comment in database. 
        /// Checks to see if comment ID is in the database.
        /// Returns false if it isn't. Edits the Comment in the DB and returns true if it is.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool AttemptEditComment(Comment comment)
        {
            if (!IsCommentInDb(comment.CommentID))
            {
                return false;
            }
            else
            {
                _dbContext.Comments.Update(comment);
                _dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Attempts to remove Comment from the database. 
        /// Checks to see if comment is in the database.
        /// Returns false if it isn't. Removes the comment from the DB and returns true if it is.
        /// </summary>
        public bool AttemptRemoveComment(int commentId)
        {
            Comment comment = GetCommentById(commentId);
            if(comment == null)
            {
                return false;
            }
            else
            {
                _dbContext.Comments.Remove(comment);
                _dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Gets all comments for a Picture given the specified Picture ID.
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllCommentsForPicture(int pictureId)
        {
            return _dbContext.Comments.Where(x => x.PictureCommentedId == pictureId);
        }

        /// <summary>
        /// Gets all the Sub-Comments for a parent Comment given the specified Comment ID.
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllCommentsForParentComment(int commentId)
        {
            return _dbContext.Comments.Where(x => x.ParentCommentId == commentId);
        }

        /// <summary>
        /// Gets all the Comments posted by a User given the specified User ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllCommentsByUser(string userId)
        {
            return _dbContext.Comments.Where(x => x.UserCommentedId == userId);
        }

    }
}
