using Keepi.Client.Communication;
using Keepi.Client.Repositories.Base;
using Keepi.Client.Repositories.Interfaces;
using Keepi.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Keepi.Client.Repositories.Implementation
{
    public class LoginRepository : Repository, ILoginRepository
    {
        private const string URL = "api/Login";
        private readonly HttpClient _httpClient;

        public LoginRepository(HttpClient httpClient, IHttpService commService) : base(commService)
        {
            _httpClient = httpClient;
        }

        public async Task<List<bool>> Test() =>
             await Get<bool>(URL + System.IO.Path.AltDirectorySeparatorChar + "test");

        public async Task<List<User>> Login(string userName, string password) =>
            await Get<User>($"api/Login/_login/{userName}/{password}");
    }

}
