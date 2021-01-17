using System;
using System.Collections.Generic;
using System.Text;

namespace Space_CLib.Models
{
    class SubComment
    {
        public int SubCommentID { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public User UserComment { get; set; }

        public Comment comment { get; set; }

    }
}
