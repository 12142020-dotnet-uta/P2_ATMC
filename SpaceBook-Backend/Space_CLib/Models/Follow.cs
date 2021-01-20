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

        public DateTime Date { get; set; }
        
        public User Followed { get; set; }

        public int FollowedId { get; set; }



        public Follow()
        {
            Date = DateTime.Now;
        }

    }
}
