﻿using Microsoft.AspNetCore.Authorization;
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