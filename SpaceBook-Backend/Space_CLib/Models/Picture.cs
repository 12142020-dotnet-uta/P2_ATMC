﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class Picture :IDate
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

        public DateTime Date { get; set; }
    }
}
