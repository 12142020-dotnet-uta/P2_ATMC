using Microsoft.EntityFrameworkCore;
using SpaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Repository
{
    public class MessageRepository
    {
        public ApplicationDbContext _dbContext;

        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Return the Message that has the given ID.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public Message GetMessageById(int messageId)
        {
            return _dbContext.Messages.Include(x=>x.Recipient).Include(x=>x.Sender).FirstOrDefault(x => x.MessageID == messageId);
        }

        /// <summary>
        /// Returns all of the Messages in the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Message> GetAllMessages()
        {
            return _dbContext.Messages.Include(x => x.Recipient).Include(x => x.Sender).Include(x => x.ParentMessage);
        }

        /// <summary>
        /// Returns all of the Messages that the ApplicationUser with the given user ID has sent or received.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Message> GetMessagesByUser(string userId)
        {
            return _dbContext.Messages.Include(x => x.Recipient).Include(x => x.Sender).Where(x => x.RecipientId == userId || x.SenderId == userId);
        }

        /// <summary>
        /// Returns all of the Messages that the ApplicationUser with the given user ID has sent.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Message> GetSentMessagesByUser(string userId)
        {
            return _dbContext.Messages.Include(x=>x.Recipient).Where(x => x.SenderId == userId);
        }

        /// <summary>
        /// Returns all of the Messages that the ApplicationUser with the given user ID has received.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Message> GetReceivedMessagesByUser(string userId)
        {
            return _dbContext.Messages.Include(x=>x.Sender).Where(x => x.RecipientId == userId);
        }

        /// <summary>
        /// Checks to see if a Message with the given ID already exists in the database.
        /// Returns true if it does, returns false if it doesn't
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public bool IsMessageInDb(int messageId)
        {
            return GetMessageById(messageId) != null;
        }

        /// <summary>
        /// Checks to see if a Message in the database has the same ID as the given Message
        /// If there is, return false
        /// If there isn't, add the new Message to the database and return true
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AttemptAddMessage(Message message)
        {
            if (IsMessageInDb(message.MessageID))
            {
                return false;
            }
            else
            {
                _dbContext.Messages.Add(message);
                _dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Checks to see if a Message with the given ID exists in the database
        /// If it doesn't, return false
        /// If it does, remove that Message from the database and return true
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public bool AttemptRemoveMessage(int messageId)
        {
            Message message = GetMessageById(messageId);
            if (message == null)
            {
                return false;
            }
            else
            {
                _dbContext.Messages.Remove(message);
                _dbContext.SaveChanges();
                return true;
            }
        }

    }
}
