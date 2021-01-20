using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class SubComment: IComments, IDate
    {
        public int SubCommentID { get; set; }

        private string strText;
        public string CommentText { get => strText; set => strText = value; }

        public DateTime Date { get; set; }

        public User UserComment { get; set; }

        public Comment comment { get; set; }

        public SubComment()
        {
            Date = DateTime.Now;
        }

    }
}
