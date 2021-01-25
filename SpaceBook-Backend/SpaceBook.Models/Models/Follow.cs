using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpaceBook.Models
{
    public class Follow :IDate
    {
        [Key]
        public int FollowID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        public ApplicationUser Followed { get; set; }
        public string FollowedId { get; set; }

        public ApplicationUser Follower { get; set; }
        public string FollowerId { get; set; }



        public Follow()
        {
            Date = DateTime.Now;
        }

    }
}
