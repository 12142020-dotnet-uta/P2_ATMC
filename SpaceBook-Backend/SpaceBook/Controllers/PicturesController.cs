using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceBook.Business;
using SpaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SpaceBook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PicturesController : ControllerBase
    {
        PictureBusinessLogic _pictureBusinessLogic;
        public PicturesController(PictureBusinessLogic pictureBusinessLogic)
        {
            _pictureBusinessLogic = pictureBusinessLogic;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyPicture()
        {
            //will get daily pictures in the future; just gets picture 1 for now
            //TODO: actually get picture of the day from nasa api; probably do it in business layer
            Picture picture = await _pictureBusinessLogic.GetPicture(1);
            if (picture != null)
            {
                return Ok(picture);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("{pictureId}")]
        public async Task<IActionResult> GetPicture(int pictureId)
        {
            //get picture by id
            Picture picture = await _pictureBusinessLogic.GetPicture(pictureId);
            if (picture != null)
            {
                return Ok(picture);
            }
            else
            {
                return NotFound();
            }

        }
        [Authorize]
        [HttpDelete("{pictureId}")]
        public async Task<IActionResult> DeletePicture(int pictureId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var username = claim.Value;
            //get picture by id
            if (await _pictureBusinessLogic.DeleteUserPicture(pictureId,username))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllPictures()
        {
            var pictures = await _pictureBusinessLogic.GetAllPictures();
            if (pictures == null) { return NotFound(); }
            return Ok(pictures);
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreatePicture([FromBody] Picture userPicture)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var username = claim.Value;

            if (await _pictureBusinessLogic.CreateUserPicture(userPicture,username))
            {
                return Accepted(userPicture);
            }
            else
            {
                return BadRequest(userPicture);
            }
        }
        #region Ratings
        [HttpGet("{pictureId}/Ratings")]
        public async Task<IActionResult> GetAllRatings(int pictureId)
        {
            var ratings = await _pictureBusinessLogic.GetRatingsForPicture(pictureId);
            if (ratings==null)
            {
                return NotFound();
            }
            else
            {
                return Ok(ratings);
            }
        }

        [HttpGet("{pictureId}/Ratings/{userId}")]
        public async Task<IActionResult> GetRating(int pictureId, string userId)
        {
            var rating = await _pictureBusinessLogic.GetRating(userId, pictureId);
            if (rating == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rating);
            }
        }

        [HttpPost("{pictureId}/Ratings")]
        [Authorize]
        public async Task<IActionResult> PostRating(int pictureId, [FromBody] double rating)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var username = claim.Value;

            if( await _pictureBusinessLogic.CreateRating(username, pictureId, rating))
            {
                return Ok(rating);
            }
            else
            {
                return BadRequest(rating);
            }
        }
        [HttpPut("{pictureId}/Ratings")]
        [Authorize]
        public async Task<IActionResult> EditRating(int pictureId, [FromBody] double rating)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var username = claim.Value;

            if (await _pictureBusinessLogic.EditRating(username, pictureId, rating))
            {
                return Ok(rating);
            }
            else
            {
                return BadRequest(rating);
            }


        }
        #endregion

        #region Comments

        #endregion

    }
}
