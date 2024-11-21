using Keepi.Client.Communication;
using Keepi.Client.Repositories.Base;
using Keepi.Client.Repositories.Interfaces;
using Keepi.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Keepi.Client.Repositories.Implementation
{
    public class ProfileRepository : Repository, IProfileRepository
    {
        private const string URL = "api/Profile";

        public ProfileRepository(IHttpService commService) : base(commService)
        {
        }

        public async Task<List<User>> EditFirstName(Guid userId, string newFirstName) =>
             await Get<User>($"api/Profile/editFirstName/{userId}/{newFirstName}");

        public async Task<List<User>> EditLastName(Guid userId, string newLastName) =>
             await Get<User>($"api/Profile/editLastName/{userId}/{newLastName}");
        
        public async Task<List<User>> EditPassword(Guid userId, string newPassword) =>
             await Get<User>($"api/Profile/editPassword/{userId}/{newPassword}");
       
        public async Task<List<User>> EditCity(Guid userId, string newCity) =>
             await Get<User>($"api/Profile/editCity/{userId}/{newCity}");
      
        public async Task<List<User>> EditPhoneNumber(Guid userId, string newPhoneNumber) =>
             await Get<User>($"api/Profile/editPhoneNumber/{userId}/{newPhoneNumber}");

        public async Task<List<User>> EditAge(Guid userId, int newAge) =>
             await Get<User>($"api/Profile/editAge/{userId}/{newAge}");

        public async Task<List<User>> FollowUser(Guid _CurrentUserId, Guid _UserIdToFollow) =>
            await Get<User>($"api/Profile/followUser/{_CurrentUserId}/{_UserIdToFollow}");

        public async Task<List<User>> UnFollowUser(Guid _CurrentUserId, Guid _UserIdToUnFollow) =>
            await Get<User>($"api/Profile/unFollowUser/{_CurrentUserId}/{_UserIdToUnFollow}");

        public async Task<List<User>> GetUserFollowersList(Guid userId) =>
             await Get<User>($"api/Profile/getFollowers/{userId}");
        
        public async Task<List<User>> GetUserFollowingList(Guid userId) =>
             await Get<User>($"api/Profile/getFollowing/{userId}");
              
        public async Task<List<string>> GetUserProfileImagePath(Guid userId) =>
             await Get<string>($"api/Profile/getProfilePath/{userId}");

        public async Task<List<User>> GetUser(Guid userId) =>
             await Get<User>($"api/Profile/getUser/{userId}");

    }

}
