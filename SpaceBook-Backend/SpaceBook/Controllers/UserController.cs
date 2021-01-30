using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpaceBook.Models;
using SpaceBook.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpaceBook.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public ApplicationUserRepository _userRepo;
        public FavoriteRepository _favoriteRepo;
        public FollowRepository _followRepo;
        public PictureRepository _pictureRepo;
        public CommentRepository _commentRepo;
        public UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationUserRepository userRepo,
            FavoriteRepository favoriteRepo,
            FollowRepository followRepo,
            PictureRepository pictureRepo,
            UserManager<ApplicationUser> userManager)
        {
            _userRepo = userRepo;
            _favoriteRepo = favoriteRepo;
            _followRepo = followRepo;
            _pictureRepo = pictureRepo;
            _userManager = userManager;
        }

        //FOR AUTHORIZATION: Uncomment Authorize
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
                return Ok(await _userRepo.GetAllUsers());
        }

        [Authorize]
        [HttpGet("User")]
        public async Task<ActionResult> GetLoggedIn()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            return Ok(await _userRepo.GetUserByUsername(claim.Value));
        }


        [HttpGet("Id/{userId}")]
        public async Task<ActionResult> GetUserById(string userId)
        {
                if (await _userRepo.IsUserInDb(userId) == false)
                {
                    return NotFound();
                }
                else
                {
                return Ok(await _userRepo.GetUserById(userId));
                }
        }

        [Authorize]
        [HttpDelete("Id/{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);

            if (await _userRepo.IsUserInDb(userId) == false)
            {
                return NotFound();
            }
            else if(loggedIn.Id != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            return Ok(await _userRepo.AttemptRemoveApplicationUser(userId));
        }

        [Authorize]
        [HttpPut("Id/{userId}")]
        public async Task<ActionResult> EditUser(string userId, [FromBody] EditUserModel model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);
            if (await _userRepo.IsUserInDb(userId) == false)
            {
                return NotFound();
            }
            else if (loggedIn.Id != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            else {
                ApplicationUser user = loggedIn;
                if (model.FirstName != null)
                {
                    user.FirstName = model.FirstName;
                }
                if (model.LastName != null)
                {
                    user.LastName = model.LastName;
                }
                if (model.Email != null)
                {
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email.ToUpper();
                }
                if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.OldPassword))
                {
                    await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                }
                return Ok(await _userRepo.AttemptEditApplicationUser(user));
            }
        }

        [HttpGet("Username/{username}")]
        public async Task<ActionResult> GetUserByUsername(string username)
        {
            if (await _userRepo.GetUserByUsername(username) == null)
            {
                return NotFound();
            }
            return Ok(await _userRepo.GetUserByUsername(username));
        }

        [HttpGet("Id/{userId}/Favorites")]
        public async Task<ActionResult> GetAllFavoritesOfUser(string userId)
        {
            if (await _userRepo.IsUserInDb(userId) == false)
            {
                return NotFound();
            }
            return Ok(await _favoriteRepo.GetFavoritesByUser(userId));
        }

        [HttpGet("Id/{userId}/Followers")]
        public async Task<ActionResult> GetAllFollowersOfUser(string userId)
        {
            if (await _userRepo.IsUserInDb(userId) == false)
            {
                return NotFound();
            }
            return Ok(await _followRepo.GetFollowersOfUser(userId));
        }

        [HttpGet("Id/{userId}/Followed")]
        public async Task<ActionResult> GetAllFollowedOfUser(string userId)
        {
            if (await _userRepo.IsUserInDb(userId) == false)
            {
                return NotFound();
            }
            return Ok(await _followRepo.GetFollowedOfUser(userId));
        }

        //FOR AUTHORIZATION: Uncomment Authorize, Remove followerUserId paramater
        [Authorize]
        [HttpPost("Id/{followedUserId}/Follow")]
        public async Task<ActionResult> FollowUser(string followedUserId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);


            //FOR AUTHORIZATION: Remove followerUserId from if statement, Add loggedIn.Id 
            if (await _userRepo.IsUserInDb(followedUserId) == false || await _userRepo.IsUserInDb(loggedIn.Id) == false)
            {
                return NotFound();
            }
            else
            {
                //FOR AUTHORIZATION: Remove following line, uncomment follower = loggedIn
                //ApplicationUser follower = await _userRepo.GetUserById(followerUserId);
                ApplicationUser follower = loggedIn;
                ApplicationUser followed = await _userRepo.GetUserById(followedUserId);
                Follow follow = new Follow()
                {
                    Followed = followed,
                    Follower = follower,
                    FollowerId = follower.Id,
                    FollowedId = followedUserId,
                };
                return Ok(await _followRepo.AttemptAddFollow(follow));
            }
        }

        //FOR AUTHORIZATION: Uncomment Authorize, Remove followerUserId paramater
        [Authorize]
        [HttpDelete("Id/{followedUserId}/Follow")]
        public async Task<ActionResult> DeleteFollow(string followedUserId)
        {
            //FOR AUTHORIZATION: Uncomment following 3 lines
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);

            //FOR AUTHORIZATION: Remove followerUserId from if statement, Add loggedIn.Id 
            if (await _userRepo.IsUserInDb(followedUserId) == false || await _userRepo.IsUserInDb(loggedIn.Id) == false)
            {
                return NotFound();
            }
            else
            {
                //FOR AUTHORIZATION: Remove followerUserId parameter, Add loggedIn.Id  
                Follow follow = await _followRepo.GetFollowByFollowerAndFollowedIds(loggedIn.Id, followedUserId);
                return Ok(await _followRepo.AttemptRemoveFollow(follow.FollowID));
            }

        }

        //FOR AUTHORIZATION: Uncomment Authorize
        [Authorize]
        [HttpPost("Id/{userId}/Favorites")]
        public async Task<ActionResult> PostFavorite(string userId, [FromBody] int pictureId)
        {
            //FOR AUTHORIZATION: Uncomment following 3 lines
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);


            ApplicationUser user = await _userRepo.GetUserById(userId);
            Picture picture = await _pictureRepo.GetPictureById(pictureId);

            //FOR AUTHORIZATION: Uncomment following 3 lines
            if (loggedIn.Id != userId)
            {
                return Forbid();
            }
            Favorite favorite = new Favorite()
            {
                User = user,
                Picture = picture,
                UserId = userId,
                PictureId = pictureId
            };
            return Ok(await _favoriteRepo.AttemptAddFavorite(favorite));
        }



        [Authorize]
        [HttpDelete("Id/{userId}/Favorites")]
        public async Task<ActionResult> RemoveFavorite(string userId, [FromBody] int pictureId)
        {
            //FOR AUTHORIZATION: Uncomment following 6 commented lines
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);

            if (loggedIn.Id != userId)
            {
                return Forbid();
            }

            Favorite favorite = await _favoriteRepo.GetFavoriteByUserPicture(userId, pictureId);
            return Ok(await _favoriteRepo.AttemptRemoveFavorite(favorite.FavoriteID));
        }

    }
}
