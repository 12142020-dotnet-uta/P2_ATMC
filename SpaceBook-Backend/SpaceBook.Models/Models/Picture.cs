using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SpaceBook.Models
{
    public class Picture :IDate
    {

        [Key] 
        public int PictureID { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        public MediaType MediaType { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageURL { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
