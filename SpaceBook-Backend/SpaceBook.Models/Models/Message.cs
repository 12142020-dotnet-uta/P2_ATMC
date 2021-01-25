using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpaceBook.Models
{
    public class Message: IDate
    {
        [Key]
        public int MessageID { get; set; }

        [DataType(DataType.Text)]
        [StringLength(300, ErrorMessage = "The comment must not exceed 300 characters.")]
        [Required]
        public string Text { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ApplicationUser Sender { get; set; }

        public string SenderId { get; set; }

        public ApplicationUser Recipient { get; set; }

        public string RecipientId { get; set; }

        public Message ParentMessage { get; set; }


        public Message()
        {
            Date = DateTime.Now;
        }

    }
}
