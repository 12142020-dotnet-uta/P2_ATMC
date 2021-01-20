using System;
namespace SpaceBook.Models
{
    public class MessageRecipient
    {
        public int MessageRecipientId { get; set; }

        public User Recipient { get; set; }

        public int RecipientId { get; set; }

        public Message Message { get; set; }

        public int MessageId { get; set; }

    }
}
