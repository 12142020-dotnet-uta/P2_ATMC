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
        public ActionResult GetAllUsers()
        {
            return Ok(_userRepo.GetAllUsers());
        }

        [HttpGet("Id/{userId}")]
        public ActionResult GetUserById(string userId)
        {
            if (!_userRepo.IsUserInDb(userId))
            {
                return NotFound();
            }
            return Ok(_userRepo.GetUserById(userId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("Id/{userId}")]
        public ActionResult DeleteUser(string userId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = _userRepo.GetUserByUsername(claim.Value);

            if (!_userRepo.IsUserInDb(userId))
            {
                return NotFound();
            }else if(loggedIn.Id != userId){
                return Forbid();
            }
            return Ok(_userRepo.AttemptRemoveApplicationUser(userId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("Id/{userId}")]
        public ActionResult EditUser(string userId, [FromBody] EditUserModel model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = _userRepo.GetUserByUsername(claim.Value);
            if (!_userRepo.IsUserInDb(userId))
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
                _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                return Ok(_userRepo.AttemptEditApplicationUser(user));
            }
        }

        [HttpGet("Username/{username}")]
        public ActionResult GetUserByUsername(string username)
        {
            if (_userRepo.GetUserByUsername(username) == null)
            {
                return NotFound();
            }
            return Ok(_userRepo.GetUserByUsername(username));
        }

        [HttpGet("Id/{userId}/Favorites")]
        public ActionResult GetAllFavoritesOfUser(string userId)
        {
            if (!_userRepo.IsUserInDb(userId))
            {
                return NotFound();
            }
            return Ok(_favoriteRepo.GetFavoritesByUser(userId));
        }

        [HttpGet("Id/{userId}/Followers")]
        public ActionResult GetAllFollowersOfUser(string userId)
        {
            if (!_userRepo.IsUserInDb(userId))
            {
                return NotFound();
            }
            return Ok(_followRepo.GetFollowersOfUser(userId));
        }

        [HttpGet("Id/{userId}/Followed")]
        public ActionResult GetAllFollowedOfUser(string userId)
        {
            if (!_userRepo.IsUserInDb(userId))
            {
                return NotFound();
            }
            return Ok(_followRepo.GetFollowedOfUser(userId));
        }

        [HttpPost("Id/{followedUserId}/Follow")]
        public ActionResult FollowUser(string followedUserId, [FromBody] string followerUserId)
        {
            if (!_userRepo.IsUserInDb(followedUserId) || !_userRepo.IsUserInDb(followerUserId))
            {
                return NotFound();
            }
            else
            {
                ApplicationUser follower = _userRepo.GetUserById(followerUserId);
                ApplicationUser followed = _userRepo.GetUserById(followedUserId);
                Follow follow = new Follow()
                {
                    Followed = followed,
                    Follower = follower,
                    FollowerId = followerUserId,
                    FollowedId = followedUserId,
                };
                return Ok(_followRepo.AttemptAddFollow(follow));
            }
        }

        [HttpDelete("Id/{followedUserId}/Follow")]
        public ActionResult DeleteFollow(string followedUserId, [FromBody] string followerUserId)
        {
            if (!_userRepo.IsUserInDb(followedUserId) || !_userRepo.IsUserInDb(followerUserId))
            {
                return NotFound();
            }
            else
            {
                Follow follow = _followRepo.GetFollowByFollowerAndFollowedIds(followerUserId, followedUserId);
                return Ok(_followRepo.AttemptRemoveFollow(follow.FollowID));
            }

        }


        [HttpPost("Id/{userId}/Favorites")]
        public ActionResult PostFavorite(string userId, [FromBody]int pictureId)
        {
            ApplicationUser user = _userRepo.GetUserById(userId);
            Picture picture = _pictureRepo.GetPictureById(pictureId);
            Favorite favorite = new Favorite()
            {
                User = user,
                Picture = picture,
                UserId = userId,
                PictureId = pictureId
            };
            return Ok(_favoriteRepo.AttemptAddFavorite(favorite));
        }

        [HttpDelete("Id/{userId}/Favorites")]
        public ActionResult RemoveFavorite(string userId, [FromBody] int pictureId)
        {
            Favorite favorite = _favoriteRepo.GetFavoriteByUserPicture(userId, pictureId);
            return Ok(_favoriteRepo.AttemptRemoveFavorite(favorite.FavoriteID));
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
