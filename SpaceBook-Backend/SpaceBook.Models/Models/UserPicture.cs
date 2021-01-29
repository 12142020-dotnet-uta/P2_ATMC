using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SpaceBook.Models
{
    public class UserPicture
    {
        [Key]
        public int UserPictureID { get; set; }
        public ApplicationUser UploadedBy { get; set; }
        /* save the picture as stream in db, a folder in the API resources or in a Azure Blob storage?? */

        public string UploadedById { get; set; }

        public Picture Picture { get; set; }

        public int PictureId { get; set; }

        public UserPicture()
        {
            //Picture.isUserPicture = true;
        }

    }
}
