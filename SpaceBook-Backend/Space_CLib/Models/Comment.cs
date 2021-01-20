﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class Comment: IComments, IDate
    {
        public int CommentID { get; set; }

        public string CommentText { get; set; }

        public DateTime Date { get; set; }

        public User UserCommented { get; set; }

        public Picture PictureCommented { get; set; }


        public Comment()
        {
            Date = DateTime.Now;
        }

    }
}
