using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class Rating
    {

        public int RatingID { get; set; }

        [Required]
        public double Value { get; set; }

        public User UserRating { get; set; }

        public Picture RatedPicture { get; set; }


    }
}
