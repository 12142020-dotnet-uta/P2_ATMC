using System;
using System.Collections.Generic;
using System.Text;

namespace Space_CLib.Models
{
    class Follow :IDate
    {
        public int FollowID { get; set; }

        public DateTime Date { get; set; }

        public User UserFollowed { get; set; }

        public User FollowedUser { get; set; }

        public Follow()
        {
            Date = DateTime.Now;
        }

    }
}
