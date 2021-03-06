﻿using Microsoft.EntityFrameworkCore;
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
            _dbContext = dbContext;
        }
        

        /// <summary>
        /// Returns a comment given the specified Comment ID. Returns null if no comment has that ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Comment> GetCommentById(int commentId)
        {
            return await _dbContext.Comments.Include(x=>x.PictureCommented).Include(x=>x.UserCommented).AsQueryable().FirstOrDefaultAsync<Comment>(x => x.CommentID == commentId);
        }

        /// <summary>
        /// Returns all comments in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await _dbContext.Comments.Include(x => x.PictureCommented).Include(x => x.UserCommented).AsQueryable().ToListAsync<Comment>(); ;
        }

        /// <summary>
        /// Gets all comments for a Picture given the specified Picture ID.
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetAllCommentsForPicture(int pictureId)
        {
            return await _dbContext.Comments.Include(x => x.UserCommented).Where(x => x.PictureCommentedId == pictureId).AsQueryable().ToListAsync<Comment>();
        }

        /// <summary>
        /// Gets all the Sub-Comments for a parent Comment given the specified Comment ID.
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetAllCommentsForParentComment(int commentId)
        {
            return await _dbContext.Comments.Include(x => x.UserCommented).Include(x => x.PictureCommented).Where(x => x.ParentCommentId == commentId).AsQueryable().ToListAsync<Comment>();
        }

        /// <summary>
        /// Gets all the Comments posted by a User given the specified User ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetAllCommentsByUser(string userId)
        {
            return await _dbContext.Comments.Include(x => x.PictureCommented).Where(x => x.UserCommentedId == userId).AsQueryable().ToListAsync<Comment>();
        }

        /// <summary>
        /// Returns true if the given comment ID is in the Database. Otherwise returns false.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsCommentInDb(int id)
        {
            return await GetCommentById(id) != null;
        }

        /// <summary>
        /// Attempts to Add Comment to db. 
        /// Checks to see if comment ID is already in Db. 
        /// Returns false if it is. Adds the Comment to the DB and returns true if it isn't.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<bool> AttemptAddCommentToDb(Comment comment)
        {
            if (await IsCommentInDb(comment.CommentID))
            {
                return false;
            }
            else
            {
                _dbContext.Comments.Add(comment);
                await _dbContext.SaveChangesAsync();
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
        public async Task<bool> AttemptEditComment(Comment comment)
        {
            Comment cmt = await GetCommentById(comment.CommentID);
            if (cmt == null)
            {
                return false;
            }
            else
            {
                _dbContext.Comments.Update(comment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// Attempts to remove Comment from the database. 
        /// Checks to see if comment is in the database.
        /// Returns false if it isn't. Removes the comment from the DB and returns true if it is.
        /// </summary>
        public async Task<bool> AttemptRemoveComment(int commentId)
        {
            Comment comment = await GetCommentById(commentId);
            if(comment == null)
            {
                return false;
            }
            else
            {
                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

        

    }
}
