using Keepi.Shared;

namespace Keepi.Client.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        Task<List<User>> Login(string userName, string password);
    }
}
