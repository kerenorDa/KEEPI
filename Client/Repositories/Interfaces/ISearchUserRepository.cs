using Keepi.Shared;
using Microsoft.AspNetCore.Mvc;
using static Keepi.Client.Pages.RegisterPage;

namespace Keepi.Client.Repositories.Interfaces
{
    public interface ISearchUserRepository
    {
        Task<List<User>> GetUserByUserName(string userName);

        Task<List<string>> SearchUsers(string prefix);
    }

}
