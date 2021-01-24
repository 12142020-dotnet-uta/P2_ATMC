using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceBook.Models;
using SpaceBook.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SpaceBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private ApplicationUserRepository _userRepo;
        private MessageRepository _messageRepo;

        public MessagesController(ApplicationUserRepository userRepo, MessageRepository messageRepo)
        {
            _userRepo = userRepo;
            _messageRepo = messageRepo;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetMyMessages()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = _userRepo.GetUserByUsername(claim.Value).Result;
            if (!_userRepo.IsUserInDb(loggedIn.Id).Result)
            {
                return NotFound();
            }
            return Ok(await _messageRepo.GetMessagesByUser(loggedIn.Id));
        }

        [Authorize]
        [HttpGet("User/{userId}")]
        public async Task<ActionResult> GetMessagesBetweenUser(string userId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = _userRepo.GetUserByUsername(claim.Value).Result;
            if (!_userRepo.IsUserInDb(loggedIn.Id).Result || !_userRepo.IsUserInDb(userId).Result)
            {
                return NotFound();
            }
            else
            {
                return Ok(await _messageRepo.GetMessagesBetween2Users(userId, loggedIn.Id));
            }
            
        }

        [Authorize]
        [HttpPost("User/{userId}")]
        public async Task<ActionResult> PostMessageToUser(string userId, [FromBody] string messageBody)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = _userRepo.GetUserByUsername(claim.Value).Result;
            if (!_userRepo.IsUserInDb(loggedIn.Id).Result || !_userRepo.IsUserInDb(userId).Result)
            {
                return NotFound();
            }
            else
            {
                ApplicationUser recipient = _userRepo.GetUserById(userId).Result;
                Message parentMessage = null;
                if (_messageRepo.GetMessagesBetween2Users(userId, loggedIn.Id).Result.Count() != 0)
                {
                    parentMessage = _messageRepo.GetMessagesBetween2Users(userId, loggedIn.Id).Result.Last();
                }
                Message message = new Message()
                {
                    Text = messageBody,
                    Sender = loggedIn,
                    SenderId = loggedIn.Id,
                    Recipient = recipient,
                    RecipientId = recipient.Id,
                    Date = DateTime.Now,
                    ParentMessage = parentMessage,
                };
                return Ok(await _messageRepo.AttemptAddMessage(message));
            }

        }
    }
}
