using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Models.ViewModels
{
    public class UserPictureViewModel
    {
        //public string username { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string fileAsBase64 { get; set; }
        public string fileExtension { get; set; }
        public byte[] fileAsByteArray { get; set; }

        

    }
}
