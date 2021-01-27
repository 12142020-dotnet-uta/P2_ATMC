using SpaceBook.Models;
using SpaceBook.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Business
{
    public class Mapper
    {
        public Picture ConvertAPOD_PictureViewModelIntoPictureModel(APOD_PictureViewModel pictureViewModel)
        {
            MediaType mediaType = new MediaType();
            MediaType.TryParse(pictureViewModel.media_type, out mediaType);
            Picture picture = new Picture()
            {
                Date = pictureViewModel.date,
                Description = pictureViewModel.explanation,
                ImageURL = pictureViewModel.url,
                ImageHDURL = pictureViewModel.hdurl,
                MediaType = mediaType,
                Title = pictureViewModel.title,
            };
            return picture;
        }
    }
}
