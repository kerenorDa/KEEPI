using Keepi.Shared;

namespace Keepi.Client.Repositories.Interfaces
{
    public interface IWalletRepository
    {
        Task<List<int>> Deposit(Guid userId, int count);

        Task<List<int>> Withdrawal(Guid userId, int count);

        Task<List<bool>> DepositToOtherUser(Guid user1_id, Guid user2_id, int count);
    }
}
