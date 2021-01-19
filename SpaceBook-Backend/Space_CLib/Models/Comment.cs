using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class Comment
    {
        public int CommentID { get; set; }

        [StringLength(300, ErrorMessage = "The comment must not exceed 300 characters.")]
        [DataType(DataType.Text)]
        [Required]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public User UserCommented { get; set; }

        public Picture PictureCommented { get; set; }


        public Comment()
        {
            Date = DateTime.Now;
        }

    }
}
