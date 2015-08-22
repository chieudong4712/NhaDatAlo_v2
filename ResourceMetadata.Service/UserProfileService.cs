using ResourceMetadata.Data.Infrastructure;
using ResourceMetadata.Data.Repositories;
using ResourceMetadata.Model;
using System;

namespace ResourceMetadata.API.Controllers
{
    public interface IUserProfileService
    {
        UserProfile GetProfile(int id);
        UserProfile GetProfile(string userid);
        
        void CreateUserProfile(string userId);
        void UpdateUserProfile(UserProfile user);
        void SaveUserProfile();
    }
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserProfileService(IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            this.userProfileRepository = userProfileRepository;
            this.unitOfWork = unitOfWork;
        }

        public UserProfile GetProfile(int id)
        {
            var userprofile = userProfileRepository.Get(u => u.Id == id);
            return userprofile;
        }
        public UserProfile GetProfile(string userid)
        {
            var userprofile = userProfileRepository.Get(u => u.UserId == userid);
            return userprofile;
        }

        public void CreateUserProfile(string userId)
        {

            UserProfile newUserProfile = new UserProfile();
            newUserProfile.UserId = userId;
            userProfileRepository.Add(newUserProfile);
            SaveUserProfile();
        }
        public void UpdateUserProfile(UserProfile user)
        {
            userProfileRepository.Update(user);
            SaveUserProfile();
        }
        public void SaveUserProfile()
        {
            unitOfWork.SaveChanges();
        }
    }
}
