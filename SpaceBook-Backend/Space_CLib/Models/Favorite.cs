using System;
using System.Collections.Generic;
using System.Text;

namespace Space_CLib.Models
{
    class Favorite
    {
        public int FavoriteID { get; set; }
        public User user { get; set; }
        public Picture picture { get; set; }
    }
}
