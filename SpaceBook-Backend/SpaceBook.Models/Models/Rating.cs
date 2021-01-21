using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SpaceBook.Models
{
    public class Rating
    {
        [Key]
        public int RatingID { get; set; }

        [Required]
        [Range(0,5)]
        public double Value { get; set; }

        public ApplicationUser UserRating { get; set; }

        public string UserRatingId { get; set; }

        public Picture RatedPicture { get; set; }

        public int RatedPictureId { get; set; }



    }
}
