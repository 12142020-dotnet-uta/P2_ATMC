using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SpaceBook.Models
{
   public class Comment: IComments, IDate
    {
        [Key]
        public int CommentID { get; set; }
        [StringLength(300, ErrorMessage = "The comment must not exceed 300 characters.")]
        [DataType(DataType.Text)]
        [Required]
        public string CommentText { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ApplicationUser UserCommented { get; set; }

        public string UserCommentedId { get; set; }


        public Picture PictureCommented { get; set; }

        public int PictureCommentedId { get; set; }

        public Comment ParentComment { get; set; }

        public int ParentCommentId { get; set; }

        public Comment()
        {
            Date = DateTime.Now;
        }

    }
}
