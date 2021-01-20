using System;
using System.ComponentModel.DataAnnotations;

namespace SpaceBook.Models
{
    public class Follower
    {
        [Key]
      public int FollowerId { get; set; }

      public Follow Follow { get; set; }

      public User User { get; set; }

        
    }
}
