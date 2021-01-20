using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Space_CLib.Models
{
    class UserPicture
    {
        [Key]
        public int UserPictureID { get; set; }
        public User UploadedBy { get; set; }
        /* save the picture as stream in db, a folder in the API resources or in a Azure Blob storage?? */
        public Picture Picture { get; set; }
    }
}
