using System;
using System.Collections.Generic;
using System.Text;

namespace Space_CLib.Models
{
    class Comment
    {
        public int CommentID { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public User UserCommented { get; set; }

        public Picture PictureCommented { get; set; }

    }
}
