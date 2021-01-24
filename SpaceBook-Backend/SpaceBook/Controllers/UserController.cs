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

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
                return Ok(await _userRepo.GetAllUsers());
        }

        [HttpGet("Id/{userId}")]
        public async Task<ActionResult> GetUserById(string userId)
        {
                if (!_userRepo.IsUserInDb(userId).Result)
                {
                    return NotFound();
                }
                else
                {
                return Ok(await _userRepo.GetUserById(userId));
                }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("Id/{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = _userRepo.GetUserByUsername(claim.Value).Result;

            if (!_userRepo.IsUserInDb(userId).Result)
            {
                return NotFound();
            }
            else if(loggedIn.Id != userId)
            {
                return Forbid();
            }
            return Ok(await _userRepo.AttemptRemoveApplicationUser(userId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("Id/{userId}")]
        public async Task<ActionResult> EditUser(string userId, [FromBody] EditUserModel model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = _userRepo.GetUserByUsername(claim.Value).Result;
            if (!_userRepo.IsUserInDb(userId).Result)
            {
                return NotFound();
            }
            else if (loggedIn.Id != userId)
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
                await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                return Ok(await _userRepo.AttemptEditApplicationUser(user));
            }
        }

        [HttpGet("Username/{username}")]
        public async Task<ActionResult> GetUserByUsername(string username)
        {
            if (_userRepo.GetUserByUsername(username).Result == null)
            {
                return NotFound();
            }
            return Ok(await _userRepo.GetUserByUsername(username));
        }

        [HttpGet("Id/{userId}/Favorites")]
        public async Task<ActionResult> GetAllFavoritesOfUser(string userId)
        {
            if (!_userRepo.IsUserInDb(userId).Result)
            {
                return NotFound();
            }
            return Ok(await _favoriteRepo.GetFavoritesByUser(userId));
        }

        [HttpGet("Id/{userId}/Followers")]
        public async Task<ActionResult> GetAllFollowersOfUser(string userId)
        {
            if (!_userRepo.IsUserInDb(userId).Result)
            {
                return NotFound();
            }
            return Ok(await _followRepo.GetFollowersOfUser(userId));
        }

        [HttpGet("Id/{userId}/Followed")]
        public async Task<ActionResult> GetAllFollowedOfUser(string userId)
        {
            if (!_userRepo.IsUserInDb(userId).Result)
            {
                return NotFound();
            }
            return Ok(await _followRepo.GetFollowedOfUser(userId));
        }

        [HttpPost("Id/{followedUserId}/Follow")]
        public async Task<ActionResult> FollowUser(string followedUserId, [FromBody] string followerUserId)
        {
            if (!_userRepo.IsUserInDb(followedUserId).Result || !_userRepo.IsUserInDb(followerUserId).Result)
            {
                return NotFound();
            }
            else
            {
                ApplicationUser follower = _userRepo.GetUserById(followerUserId).Result;
                ApplicationUser followed = _userRepo.GetUserById(followedUserId).Result;
                Follow follow = new Follow()
                {
                    Followed = followed,
                    Follower = follower,
                    FollowerId = followerUserId,
                    FollowedId = followedUserId,
                };
                return Ok(await _followRepo.AttemptAddFollow(follow));
            }
        }

        [HttpDelete("Id/{followedUserId}/Follow")]
        public async Task<ActionResult> DeleteFollow(string followedUserId, [FromBody] string followerUserId)
        {
            if (!_userRepo.IsUserInDb(followedUserId).Result || !_userRepo.IsUserInDb(followerUserId).Result)
            {
                return NotFound();
            }
            else
            {
                Follow follow = _followRepo.GetFollowByFollowerAndFollowedIds(followerUserId, followedUserId).Result;
                return Ok(await _followRepo.AttemptRemoveFollow(follow.FollowID));
            }

        }


        [HttpPost("Id/{userId}/Favorites")]
        public async Task<ActionResult> PostFavorite(string userId, [FromBody]int pictureId)
        {
            ApplicationUser user = _userRepo.GetUserById(userId).Result;
            Picture picture = _pictureRepo.GetPictureById(pictureId);
            Favorite favorite = new Favorite()
            {
                User = user,
                Picture = picture,
                UserId = userId,
                PictureId = pictureId
            };
            return Ok(await _favoriteRepo.AttemptAddFavorite(favorite));
        }

        [HttpDelete("Id/{userId}/Favorites")]
        public async Task<ActionResult> RemoveFavorite(string userId, [FromBody] int pictureId)
        {
            Favorite favorite = _favoriteRepo.GetFavoriteByUserPicture(userId, pictureId).Result;
            return Ok(await _favoriteRepo.AttemptRemoveFavorite(favorite.FavoriteID));
        }


        //[HttpGet("Id/{id}/Comments")]
        //public ActionResult GetCommentsOfUser(string userId)
        //{
        //    if (!_userRepo.IsUserInDb(userId))
        //    {
        //        return NotFound();
        //    }
        //    return Ok(_commentRepo.GetAllCommentsByUser(userId));
        //}
    }
}
