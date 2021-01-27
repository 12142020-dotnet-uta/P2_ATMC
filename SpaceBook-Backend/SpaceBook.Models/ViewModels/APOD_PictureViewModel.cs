using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBook.Models.ViewModels
{
    public class APOD_PictureViewModel
    {
        public string copyright { get; set; }
        public DateTime date { get; set; }
        public string explanation { get; set; }
        public string hdurl { get; set; }
        public string url { get; set; }
        public string media_type { get; set; }
        public string service_version { get; set; }
        public string title { get; set; }
    }
}
