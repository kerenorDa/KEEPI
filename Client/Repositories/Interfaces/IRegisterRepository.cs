using Keepi.Shared;
using Microsoft.AspNetCore.Mvc;
using static Keepi.Client.Pages.RegisterPage;

namespace Keepi.Client.Repositories.Interfaces
{
    public interface IRegisterRepository
    {
        Task<List<User>> Register(string _username, string _firstName, string _lastName, string _password, string _email, string _city, int _age, string _phoneNumber);
    }

}
