using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class Favorite
    {
        [Key]
        public int FavoriteID { get; set; }

        public User User { get; set; }
        public Picture Picture { get; set; }
    }
}
