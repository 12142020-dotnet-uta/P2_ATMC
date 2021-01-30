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
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);
            if (await _userRepo.IsUserInDb(loggedIn.Id) == false)
            {
                return NotFound();
            }
            return Ok(await _messageRepo.GetMessagesByUser(loggedIn.Id));
        }

        [Authorize]
        [HttpGet("Users")]
        public async Task<ActionResult> GetUsersInConversationWithLoggedIn()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);

            if (await _userRepo.IsUserInDb(loggedIn.Id) == false)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<Message> messages = await _messageRepo.GetMessagesByUser(loggedIn.Id);
                List<ApplicationUser> userList = new List<ApplicationUser>();
                foreach(Message m in messages)
                {
                    if(m.Sender != loggedIn)
                    {
                        userList.Add(m.Sender);
                    }
                    else if(m.Recipient != loggedIn) {
                        userList.Add(m.Recipient);
                    }
                }
                return Ok(userList);
            }

        }

        [Authorize]
        [HttpGet("User/{userId}")]
        public async Task<ActionResult> GetMessagesBetweenUser(string userId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);
            if (await _userRepo.IsUserInDb(loggedIn.Id) == false || await _userRepo.IsUserInDb(userId) == false)
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
            var loggedIn = await _userRepo.GetUserByUsername(claim.Value);
            if (await _userRepo.IsUserInDb(loggedIn.Id) == false || await _userRepo.IsUserInDb(userId) == false)
            {
                return NotFound();
            }
            else
            {
                ApplicationUser recipient = await _userRepo.GetUserById(userId);
                Message parentMessage = null;
                IEnumerable<Message> messages = await _messageRepo.GetMessagesBetween2Users(userId, loggedIn.Id);
                if (messages.Count() != 0)
                {
                    parentMessage = messages.Last();
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
