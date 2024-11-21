using Keepi.Client.Communication;
using Keepi.Client.Repositories.Base;
using Keepi.Client.Repositories.Interfaces;
using Keepi.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using static Keepi.Client.Pages.RegisterPage;

namespace Keepi.Client.Repositories.Implementation
{
    public class SearchUserRepository : Repository, ISearchUserRepository
    {
        private const string URL = "api/SearchUser";

        public SearchUserRepository(IHttpService commService) : base(commService)
        {
            
        }

        //public async Task<object> Register(RegistrationModel user) =>
        //     await Post<RegistrationModel>(URL + System.IO.Path.AltDirectorySeparatorChar + "register", user);

        //public async Task<List<User>> Register(RegistrationModel user) =>
        //   await Get<User>($"api/Register/_register/{user}");

        public async Task<List<User>> GetUserByUserName(string userName) =>
           await Get<User>($"api/SearchUser/getUserByUserName/{userName}");
        
        public async Task<List<string>> SearchUsers(string prefix) =>
           await Get<string>($"api/SearchUser/searchUsers/{prefix}");
    }

}
