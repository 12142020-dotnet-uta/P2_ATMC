using System;
using System.Collections.Generic;
using System.Text;

namespace Space_CLib.Models
{
    class Picture
    {

        public int PictureID { get; set; }

        public string Title { get; set; }

        public bool MediaType { get; set; }

        public string ImageURL { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }


    }
}
