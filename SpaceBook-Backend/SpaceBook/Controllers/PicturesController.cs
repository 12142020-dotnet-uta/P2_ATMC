using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceBook.Business;
using SpaceBook.Controllers.Filter;
using SpaceBook.Controllers.Pagination;
using SpaceBook.Controllers.Services;
using SpaceBook.Controllers.Wrappers;
using SpaceBook.Models;
using SpaceBook.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SpaceBook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PicturesController : ControllerBase
    {
        PictureBusinessLogic _pictureBusinessLogic;
        UriPaginationInterface _uriService;

        public PicturesController(PictureBusinessLogic pictureBusinessLogic, UriPaginationInterface uriService)
        {
            _pictureBusinessLogic = pictureBusinessLogic;
            _uriService = uriService;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyPicture()
        {
            //will get daily pictures in the future; just gets picture 1 for now
            //TODO: actually get picture of the day from nasa api; probably do it in business layer await _pictureBusinessLogic.GetPicture(1);
            Picture picture = await _pictureBusinessLogic.GetPictureOfTheDay();
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
                //return Ok(new Response<Picture>(picture));
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
        public async Task<IActionResult> GetAllPictures(int pageNumber = 1, int pageSize = 20) //[FromQuery] PaginationFilter filter
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(pageNumber, pageSize);
            var pagedData = (await _pictureBusinessLogic.GetAllPictures(pageNumber, pageSize))
                .Skip((validFilter.PageNumber - 1)
                * validFilter.PageSize)
                .Take((validFilter.PageSize))
                .ToList();
            var pictures = (await _pictureBusinessLogic.GetAllPictures()).Count() ;
            if (pagedData == null) { return NotFound(); }

            var response = Helpers.PaginationHelper.CreatePagedResponse(pagedData, validFilter, pictures, _uriService, route);

            return Ok(response);
        }

  


        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreatePicture([FromBody] UserPictureViewModel userPicture)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var username = claim.Value;


            bool result = await _pictureBusinessLogic.CreateUserPicture(userPicture, username);

            if(result)
                return Accepted(result);
            else
                return BadRequest(result);

        }
        #region Ratings
        [HttpGet("{pictureId}/Ratings")]
        public async Task<IActionResult> GetAllRatings(int pictureId)
        {
            try
            {
                double ratingAvg = await _pictureBusinessLogic.GetRatingsForPicture(pictureId);

                if(ratingAvg == 0 || double.IsNaN(ratingAvg) )
                {
                    return NotFound("The picture have not been rated yet.");
                }
                else
                {
                    return Ok(ratingAvg);
                }
            }
            catch 
            {
                return NotFound("The picture have not been rated yet.");
            }

        }
        [Authorize]
        [HttpGet("{pictureId}/Ratings/User")]
        public async Task<IActionResult> GetRating(int pictureId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            string username = claim.Value;

            var rating = await _pictureBusinessLogic.GetRating(username, pictureId);

            if (rating == null)
            {
                return NotFound("The picture have not been rated yet by the user.");
            }
            else
            {
                return Ok(rating.Value);
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

        //this gets all of the comments. should we just get the comments directly on the picture?
        [HttpGet("{pictureId}/Comments")]
        public async Task<IActionResult> GetAllComments(int pictureId)
        {
            var comments = await _pictureBusinessLogic.GetCommentsForPicture(pictureId);
            if (comments == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(comments);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("{pictureId}/Comments")]
        public async Task<IActionResult> CreateCommentOnPicture(int pictureId, [FromBody] string text)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var username = claim.Value;

            var comment = await _pictureBusinessLogic.CreateCommentOnPicture(pictureId, username, null, text);
            if (comment!=null)
            {
                return Accepted(comment);
            }
            else
            {
                return BadRequest();
            }
        }
        [Authorize]
        [HttpPost]
        [Route("{pictureId}/Comments/{commentId}")]
        public async Task<IActionResult> CreateCommentOnComment(int pictureId, int commentId, [FromBody] string text)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var username = claim.Value;

            var comment = await _pictureBusinessLogic.CreateCommentOnPicture(pictureId, username, commentId, text);
            if (comment != null)
            {
                return Accepted(comment);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("{pictureId}/Comments/{commentId}/Comments")]
        public async Task<IActionResult> GetAllCommentsOnComment(int commentId)
        {
            var comments = await _pictureBusinessLogic.GetCommentsForComment(commentId);
            if (comments == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(comments);
            }
        }
        [Authorize]
        [HttpPut("{pictureId}/Comments/{commentId}")]
        public async Task<IActionResult> EditComment(int commentId, [FromBody] string newText)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var username = claim.Value;

            var comment = await _pictureBusinessLogic.EditComment(commentId,username, newText);
            if (comment == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(comment);
            }
        }
        [Authorize]
        [HttpDelete("{pictureId}/Comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId, [FromBody] string newText)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //make sure user is logged in
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            //get logged in user
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var username = claim.Value;


            if (await _pictureBusinessLogic.DeleteComment(commentId,username))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

    }
}
