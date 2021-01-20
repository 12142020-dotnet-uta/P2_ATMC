using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class Message: IDate
    {
        [Key]
        public int MessageID { get; set; }

        [DataType(DataType.Text)]
        [StringLength(300, ErrorMessage = "The comment must not exceed 300 characters.")]
        [Required]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }

        public Message()
        {
            Date = DateTime.Now;
        }

    }
}
