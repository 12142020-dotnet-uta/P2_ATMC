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
        public IActionResult GetDailyPicture()
        {
            //will get daily pictures in the future; just gets picture 1 for now
            //TODO: actually get picture of the day from nasa api; probably do it in business layer
            Picture picture = _pictureBusinessLogic.GetPicture(1);
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
        public IActionResult GetPicture(int pictureId)
        {
            //get picture by id
            Picture picture = _pictureBusinessLogic.GetPicture(pictureId);
            if (picture != null)
            {
                return Ok(picture);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("")]
        public IActionResult GetAllPictures()
        {
            var pictures = _pictureBusinessLogic.GetAllPictures();
            if (pictures == null) { return NotFound(); }
            return Ok(pictures);
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public IActionResult CreatePicture([FromBody] Picture userPicture)
        {
            //get logged in user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //if (claims== null)
            //{
            //    //if user isn't logged in
            //    return Unauthorized();
            //}
            //var userId = claim.Value;
            if (!claimsIdentity.IsAuthenticated) { return Unauthorized(); }
            var username = claimsIdentity.Name;
            if (_pictureBusinessLogic.CreateUserPicture(userPicture,username))
            {
                return Accepted(userPicture);
            }
            else
            {
                return BadRequest(userPicture);
            }
        }

    }
}
