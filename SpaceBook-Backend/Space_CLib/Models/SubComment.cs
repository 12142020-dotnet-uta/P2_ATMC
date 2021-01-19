using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class SubComment
    {
        public int SubCommentID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(300, ErrorMessage = "The comment must not exceed 300 characters.")]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public User UserComment { get; set; }

        public Comment comment { get; set; }

        public SubComment()
        {
            Date = DateTime.Now;
        }

    }
}
