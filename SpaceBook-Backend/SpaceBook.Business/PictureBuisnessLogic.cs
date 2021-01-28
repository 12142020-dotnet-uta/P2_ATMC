using SpaceBook.Models;
using SpaceBook.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpaceBook.Business
{
    class PictureBuisnessLogic
    {
        //private readonly ApplicationDbContext _applicationDBContext;

        private readonly PictureRepository _pictureRepository;
        private readonly HttpClient _client;
        private const string API_KEY = "71wMNdfgtD6jcpVBtmULKEqxPjbhhXLSit3mQqXu";

        public PictureBuisnessLogic(PictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
            _client = new HttpClient();
        }

        /// <summary>
        /// AS the name implies, it get a list of Pictures from the DB /*API?*/
        /// </summary>
        /// <returns>Returns a List of Pictures  from the DB and the API</returns>
        public List<Picture> GetAllPictures()
        {
            Task<List<Picture>> PicturesAsync = GetAllPicturesAsync();

            ///

            List<Picture> Pictures = PicturesAsync.Result;
            //

            return Pictures;
        }

        //Get the Pictures from the APOD API
        //  
        private async Task<List<Picture>> GetAllPicturesAsync()
        {

            try
            {
                string responseBody = await _client.GetStringAsync($"https://api.nasa.gov/planetary/apod?api_key={API_KEY}");

                //Convert the response into Picture Object
                Picture picture = JsonSerializer.Deserialize<Picture>(responseBody);

                //Mapper Class or DeserializeObject from JSON class

            }
            catch (Exception)
            {

                throw;
            }
            return null;

        }






    }
}
