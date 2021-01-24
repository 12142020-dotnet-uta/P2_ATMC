using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Repository
{
    public class ApplicationUserRepository
    {
        public ApplicationDbContext _dbContext;

        public ApplicationUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all ApplicationUsers in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _dbContext.ApplicationUsers.AsQueryable().ToListAsync<ApplicationUser>();
        }

        /// <summary>
        /// Returns an ApplicationUser given the specified user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _dbContext.ApplicationUsers.FirstOrDefaultAsync<ApplicationUser>(x => x.Id == userId);
        }

        /// <summary>
        /// Returns an ApplicationUser given the specified username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserByUsername(string username)
        {
            return await _dbContext.ApplicationUsers.FirstOrDefaultAsync<ApplicationUser>(x => x.UserName == username);
        }
        

        /// <summary>
        /// Checks to see if an ApplicationUser with the given ID exists in the database.
        /// Returns true if it does, false if it doesn't
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsUserInDb(string userId)
        {
            return await GetUserById(userId) != null;
        }

        /// <summary>
        /// Attempts to add a new ApplicationUser to the database.
        /// If an ApplicationUser already exists in the database with the same ID as the given ApplicationUser, return false.
        /// Otherwise, add the ApplicationUser to the database and return true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> AttemptAddApplicationUser(ApplicationUser user)
        {
            if (await IsUserInDb(user.Id)){
                return false;
            }
            else
            {
                _dbContext.ApplicationUsers.Add(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// Attempts to edit an ApplicationUser in the database.
        /// If an ApplicationUser with the same ID as the given ApplicationUser doesn't exist in the database it returns false.
        /// Otherwise it updates the user in the database and returns true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> AttemptEditApplicationUser(ApplicationUser user)
        {
            if (await IsUserInDb(user.Id))
            {
                _dbContext.ApplicationUsers.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to remove an ApplicationUser from the database.
        /// If there is no ApplicationUser in the database with the given ID, it returns false.
        /// Otherwise it removes the user from the database and returns true.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> AttemptRemoveApplicationUser(string userId)
        {
            ApplicationUser user = await GetUserById(userId);
            if(user == null)
            {
                return false;
            }
            else
            {
                _dbContext.ApplicationUsers.Remove(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }
    }
}
