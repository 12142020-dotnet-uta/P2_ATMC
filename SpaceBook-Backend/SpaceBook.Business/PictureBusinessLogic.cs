using SpaceBook.Models;
using SpaceBook.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceBook.Business
{
    public class PictureBusinessLogic
    {
        private readonly PictureRepository _pictureRepository;
        private readonly UserPictureRepository _userPictureRepository;
        private readonly ApplicationUserRepository _userRepository;
        public PictureBusinessLogic(PictureRepository pictureRepository, ApplicationUserRepository userRepository,UserPictureRepository userPictureRepository)
        {
            _pictureRepository = pictureRepository;
            _userRepository = userRepository;
            _userPictureRepository = userPictureRepository;
        }

        public Picture GetPicture(int pictureId)
        {
            return _pictureRepository.GetPictureById(pictureId);
        }

        public IEnumerable<Picture> GetAllPictures()
        {
            return _pictureRepository.GetAllPictures();
        }

        public async Task<bool> CreateUserPicture(Picture picture, string username)
        {
            var user =  await _userRepository.GetUserByUsername(username);

            UserPicture userPicture = new UserPicture()
            {
                Picture = picture,
                PictureId = picture.PictureID,
                UploadedById = user.Id,
                UploadedBy = user,
            };

            //do checks on picture before adding?
            //like make sure user is logged in?
            return _userPictureRepository.AttemptAddUserPictureToDb(userPicture);
        }

    }
}
