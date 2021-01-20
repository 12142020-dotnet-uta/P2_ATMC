using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class SubComment: IComments, IDate
    {
        [Key]
        public int SubCommentID { get; set; }
        [Required]
        public string CommentText { get; set; }

        public DateTime Date { get; set; }

        public User UserComment { get; set; }

        public Comment Comment { get; set; }

        public SubComment()
        {
            Date = DateTime.Now;
        }

    }
}
