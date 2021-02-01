using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SpaceBook.Models;
using SpaceBook.Models.ViewModels;
using SpaceBook.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpaceBook.Business
{
    public class PictureBusinessLogic
    {
        private readonly PictureRepository _pictureRepository;
        private readonly UserPictureRepository _userPictureRepository;
        private readonly ApplicationUserRepository _userRepository;
        private readonly RatingRepository _ratingRepository;
        private readonly FavoriteRepository _favoriteRepository;
        private readonly CommentRepository _commentRepository;
        private readonly HttpClient _client;
        private readonly Mapper _mapper;
        private const string API_KEY = "api_key=71wMNdfgtD6jcpVBtmULKEqxPjbhhXLSit3mQqXu";
        private const string APOD_NASA_API = "https://api.nasa.gov/planetary/apod?";
        private const string AZURE_BLOB = "DefaultEndpointsProtocol=https;AccountName=spacebookfiles;AccountKey=obDHJ6p5CFX/0H9UdUcQYgMjiiAQAbj5sZ0TwObhtmg9Tr4veXrK8jgkJTj8CPF7KPr6+9qYbUkttyS/WXVtYw==;EndpointSuffix=core.windows.net";

        public PictureBusinessLogic(PictureRepository pictureRepository, ApplicationUserRepository userRepository, UserPictureRepository userPictureRepository, RatingRepository ratingRepository, CommentRepository commentRepository, HttpClient httpClient, FavoriteRepository favoriteRepository, Mapper mapper)
        {
            _pictureRepository = pictureRepository;
            _userRepository = userRepository;
            _userPictureRepository = userPictureRepository;
            _ratingRepository = ratingRepository;
            _commentRepository = commentRepository;
            _client = httpClient;
            _mapper = mapper;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<Picture> GetPicture(int pictureId)
        {
            return await _pictureRepository.GetPictureById(pictureId);
        }

        public async Task<IEnumerable<Picture>> GetAllPictures(int PageNumber = 1, int PageSize = 20)
        {
            //TODO: When calling to the DB, if the count of pictures is to 20, return the Pictures, else, call the APOD API.
            List<Picture> pictures = (List<Picture>)await _pictureRepository.GetAllPictures(PageNumber, PageSize);

            if (pictures.Where(picture => picture.isUserPicture == false).Count() < (PageSize * PageNumber))
            {
                //  
                int PaginationDays = (PageNumber * (PageSize));// x
                int DaysToQuery = PaginationDays - pictures.Count;// y

                pictures.AddRange((List<Picture>)await GetPicturesFromAPOD_Async(PaginationDays, DaysToQuery));
            }

            return pictures;//await _pictureRepository.GetAllPictures(PageNumber, PageSize);
        }

        /// <summary>
        /// As the name implies, it get The Picture of the day from the NASA API, and compare to the DB for adding /*API?*/
        /// </summary>
        /// <returns>Returns a List of Pictures  from the DB and the API</returns>
        public async Task<Picture> GetPictureOfTheDay()
        {
            Picture picture = await _pictureRepository.IsPictureOfTheDayInDBAsync();
            //Verify if we have it already in the DB...
            if (picture == null)
            {
                picture = await GetPictureOfTheDayAsync();
            }
            return picture;
        }

        //Get the Pictures from the APOD API
        private async Task<Picture> GetPictureOfTheDayAsync()
        {
            try
            {
                string responseBody = await _client.GetStringAsync(APOD_NASA_API + API_KEY);
                //Convert the response into Picture Object
                //APOD_PictureViewModel pictureViewModel = JsonSerializer.Deserialize<APOD_PictureViewModel>(responseBody);
                //Mapper Class or DeserializeObject from JSON class
                Picture picture = _mapper.ConvertAPOD_PictureViewModelIntoPictureModel(JsonSerializer.Deserialize<APOD_PictureViewModel>(responseBody));

                await _pictureRepository.AttemptAddPictureToDb(picture);
                //ConvertObjectIntoPictureModel
                return picture;
            }
            catch (Exception ex)
            {
                throw new Exception("There was a problem while accessing the photo of the day. " + ex.Message);
            }
        }

        /// <summary>
        /// Get a list of pictures througth the APOD API, sending the amount of days to request the API the beggining date and a optional PageSize to determine the size of the request.
        /// </summary>
        /// <param name="PaginationDays">The number of days to navigate througth the API</param>
        /// <param name="PageSize">Optional parameter, is the total amount of resources for our List.</param>
        /// <returns>Returns a async Enumerable of Pictures</returns>
        private async Task<IEnumerable<Picture>> GetPicturesFromAPOD_Async(int PaginationDays, int DaysToQuery)
        {
            string strQueryDate = $"start_date={DateTime.Now.AddDays((PaginationDays * -1)).ToString("yyyy-MM-dd")}&end_date={DateTime.Now.AddDays((PaginationDays * -1) + DaysToQuery).ToString("yyyy-MM-dd")}&";
            try
            {
                string responseBody = await _client.GetStringAsync(APOD_NASA_API + strQueryDate + API_KEY);
                //Convert the response into Picture Object
                //IEnumerable<APOD_PictureViewModel> picturesViewModel = JsonSerializer.Deserialize<IEnumerable<APOD_PictureViewModel>>(responseBody);
                //Mapper Class or DeserializeObject from JSON class
                IEnumerable<Picture> pictures = ConvertEnumerableAPOD_PictureIntoEnumerablePicture(JsonSerializer.Deserialize<IEnumerable<APOD_PictureViewModel>>(responseBody));

                //ConvertObjectIntoPictureModel
                return pictures;
            }
            catch (Exception ex)
            {
                throw new Exception("There was a problem while accessing the photo of the day. " + ex.Message);
            }
        }

        private List<Picture> ConvertEnumerableAPOD_PictureIntoEnumerablePicture(IEnumerable<APOD_PictureViewModel> picturesViewModel)
        {
            List<Picture> pictures = new List<Picture>();
            foreach (APOD_PictureViewModel pictureVM in picturesViewModel)
            {
                Picture picture = _mapper.ConvertAPOD_PictureViewModelIntoPictureModel(pictureVM);
                Task<bool> AttemptAddPicture = _pictureRepository.AttemptAddPictureToDb(picture);
                pictures.Add(picture);
                AttemptAddPicture.Wait();
            }
            return pictures;
        }

        public async Task<IEnumerable<Favorite>> GetFavorites(int pictureId){
            return await _favoriteRepository.GetFavoritesByPicture(pictureId);
        }

        #region User Pictures
        public async Task<int> CreateUserPicture(UserPictureViewModel pictureVM, string username)
        {

            //Check to ensure the file type is there: 
            if (pictureVM.fileAsBase64.Contains(","))
            {
                pictureVM.fileAsBase64 = pictureVM.fileAsBase64.Substring(pictureVM.fileAsBase64.IndexOf(",") + 1);
            }
            //Convert to binary array
            pictureVM.fileAsByteArray = Convert.FromBase64String(pictureVM.fileAsBase64);
            
            string fileName = $"{pictureVM.title}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm")}.{pictureVM.fileExtension}";

            BlobContainerClient blobContainer = new BlobContainerClient(AZURE_BLOB, "images");
            blobContainer.CreateIfNotExists(PublicAccessType.Blob);

            BlobClient blockBlob = blobContainer.GetBlobClient(fileName);
            //using (var fileStream = new FileStream())
            using (Stream stream = new MemoryStream(pictureVM.fileAsByteArray))
            {

                blockBlob.Upload(stream);
            }
            string urlImage = blockBlob.Uri.AbsoluteUri;

            var user = await _userRepository.GetUserByUsername(username);
            //Task<ApplicationUser> user = _userRepository.GetUserByUsername(username);
            //user.Wait();

            Picture picture = new Picture()
            {
                Title = pictureVM.title,
                Description = pictureVM.description,
                isUserPicture = true,
                /* ----- */
                MediaType = MediaType.image,
                /* -----    */
                ImageURL = urlImage,
                ImageHDURL = urlImage,
                Date = DateTime.Now.Date,
            };

            //Save the picture first, then we link the user to the picture
            await _pictureRepository.AttemptAddPictureToDb(picture);

            UserPicture userPicture = new UserPicture()
            {
                Picture = picture,
                PictureId = picture.PictureID,
                UploadedById = user.Id,
                UploadedBy = user,
            };
            await _userPictureRepository.AttemptAddUserPictureToDb(userPicture);


            return picture.PictureID;
        }

        /// <summary>
        /// Takes a picture Id, and attmepts to delete both the user picture(relationship model) and the picture(object model) from the db
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserPicture(int pictureId, string username)
        {
            var userPicture = await _userPictureRepository.GetUserPictureByPicture(await _pictureRepository.GetPictureById(pictureId));
            if(userPicture.UploadedBy.UserName != username)
            {
                //users can only delete their own pictures.
                return false;
            }
            //delete userpicture
            if (await _userPictureRepository.AttemptRemoveUserPictureFromDb(userPicture.UserPictureID))
            {
                //delete comments and ratings on picture
                foreach (Comment comment in await _commentRepository.GetAllCommentsForPicture(userPicture.PictureId))
                {
                    await _commentRepository.AttemptRemoveComment(comment.CommentID);
                }
                foreach (Rating rating in await _ratingRepository.GetRatingsForPicture(userPicture.PictureId))
                {
                    await _ratingRepository.AttemptRemoveRating(rating.RatingID);
                }
                //if user picture is deleted, delete picture and return whether or not it was successful
                return await _pictureRepository.AttemptRemovePictureFromDb(userPicture.PictureId);
            }
            else
            {
                //the attempt to remove the user picture failed
                return false;
            }
        }
        #endregion

        #region Ratings
        /// <summary>
        /// Takes the username of the user creating the rating, the id of the picture being rated, and the value of the rating, and creates a new rating
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pictureId"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        public async Task<bool> CreateRating(string username, int pictureId, double rating)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var picture = await _pictureRepository.GetPictureById(pictureId);

            //var user = await userTask;
            //var picture = await pictureTask;

            if (user == null || picture == null)
            {
                return false;
            }
            Rating newRating = new Rating()
            {
                Value = rating,
                RatedPicture = picture,
                RatedPictureId = picture.PictureID,
                UserRating = user,
                UserRatingId = user.Id
            };
            return await _ratingRepository.AttemptAddRating(newRating);
        }
        /// <summary>
        /// edits a rating in the db based on the username, pictureId, and new value for the rating
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pictureId"></param>
        /// <param name="newRating"></param>
        /// <returns></returns>
        public async Task<bool> EditRating(string username, int pictureId, double newRating)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null) { 
                //invalid user
                return false; 
            }
            var retrievedRating = await _ratingRepository.GetRatingByPictureAndUser(pictureId, user.Id);
            if (retrievedRating == null)
            {
                //no rating found
                return false;
            }
            retrievedRating.Value = newRating;
            return await _ratingRepository.AttemptEditRating(retrievedRating); 
        }

        public async Task<Rating> GetRating(string username, int pictureId)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                //invalid user
                return null;
            }
            return await _ratingRepository.GetRatingByPictureAndUser(pictureId,user.Id);
        }

        //public async Task<IEnumerable<Rating>> GetRatingsForPicture(int pictureId)
        //{
        //    return await _ratingRepository.GetRatingsForPicture(pictureId);
        //}

        public async Task<double> GetRatingsForPicture(int pictureId)
        {
            List<Rating> ratings = (List<Rating>) await _ratingRepository.GetRatingsForPicture(pictureId);

            return GetRatingAverageFromPicture(ratings);
        }

        private double GetRatingAverageFromPicture(List<Rating> ratings)
        {

            int star5 = GetRatingCountFromPicture(ratings, 5);
            int star4 = GetRatingCountFromPicture(ratings, 4);
            int star3 = GetRatingCountFromPicture(ratings, 3);
            int star2 = GetRatingCountFromPicture(ratings, 2);
            int star1 = GetRatingCountFromPicture(ratings, 1);

            double ratingAverage = (double)(5 * star5 + 4 * star4 + 3 * star3 + 2 * star2 + 1 * star1)
                /
                (star1 + star2 + star3 + star4 + star5);

            ratingAverage = Math.Round(ratingAverage, 1);
            return ratingAverage;
        }

        private int GetRatingCountFromPicture(List<Rating> ratings, int value)
        {
            return ratings.Where(rating => rating.Value == value).Count();
        }

        #endregion

        #region Comments

        public async Task<Comment> CreateCommentOnPicture(int pictureId, string username, int? parentCommentId, string commentText)
        {
            ApplicationUser user = await _userRepository.GetUserByUsername(username);
            Picture picture = await _pictureRepository.GetPictureById(pictureId);
            Comment parentComment = null;
            if (parentCommentId != null)
            {
                if (await _commentRepository.IsCommentInDb((int)parentCommentId))
                {
                    parentComment = await _commentRepository.GetCommentById((int)parentCommentId);
                }
                else
                {
                    //trying to comment on a comment that doesn't exist
                    return null;
                }
            }

            Comment comment = new Comment()
            {
                CommentText = commentText,
                UserCommented = user,
                UserCommentedId = user.Id,
                PictureCommented = picture,
                PictureCommentedId = picture.PictureID,
                ParentComment = parentComment,
                ParentCommentId = parentCommentId
            };
            if (await _commentRepository.AttemptAddCommentToDb(comment))
            {
                return comment;
            }
            else return null;
        }
        /// <summary>
        /// Gets all comments for the given picture id
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetCommentsForPicture(int pictureId)
        {
            return await _commentRepository.GetAllCommentsForPicture(pictureId);
        }
        /// <summary>
        /// Gets all child comments for the given comment id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetCommentsForComment(int commentId)
        {
            return await _commentRepository.GetAllCommentsForParentComment(commentId);
        }
        public async Task<bool> DeleteComment(int commentId, string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var comment = await _commentRepository.GetCommentById(commentId);
            if (user == null || comment == null)
            {
                return false;
            }

            var childComments =await _commentRepository.GetAllCommentsForParentComment(commentId);
            foreach(Comment child in childComments)
            {
                child.ParentComment = comment.ParentComment;
                child.ParentCommentId = comment.ParentCommentId;
                await _commentRepository.AttemptEditComment(child);
            }
            return await _commentRepository.AttemptRemoveComment(commentId);
        }

        public async Task<Comment> EditComment(int commentId,string username, string commentText)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var comment = await _commentRepository.GetCommentById(commentId);
            if (user == null || comment == null)
            {
                return null;
            }

            comment.CommentText = commentText;
            //update time?
            if (await _commentRepository.AttemptEditComment(comment))
            {
                return comment;
            }
            else return null;
        }


        #endregion
    }
}
