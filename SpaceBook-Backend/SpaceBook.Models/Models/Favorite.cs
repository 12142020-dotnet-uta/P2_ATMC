using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SpaceBook.Models
{
    public class Favorite
    {
        [Key]
        public int FavoriteID { get; set; }

        public ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public Picture Picture { get; set; }

        public int PictureId { get; set; }

    }
}
