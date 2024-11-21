using Keepi.Client.Communication;
using Keepi.Client.Repositories.Base;
using Keepi.Client.Repositories.Interfaces;
using Keepi.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Keepi.Client.Repositories.Implementation
{
    public class WalletRepository : Repository, IWalletRepository
    {
        private const string URL = "api/Wallet";
        private readonly HttpClient _httpClient;

        public WalletRepository(HttpClient httpClient, IHttpService commService) : base(commService)
        {
            _httpClient = httpClient;
        }

        public async Task<List<int>> Deposit(Guid userId, int count) =>
             await Get<int>(URL + System.IO.Path.AltDirectorySeparatorChar + $"deposit/{userId}/{count}");

        public async Task<List<int>> Withdrawal(Guid userId, int count) =>
             await Get<int>(URL + System.IO.Path.AltDirectorySeparatorChar + $"withdrawal/{userId}/{count}");

        public async Task<List<bool>> DepositToOtherUser(Guid user1_id, Guid user2_id, int count) =>
             await Get<bool>(URL + System.IO.Path.AltDirectorySeparatorChar + $"depositToOtherUser/{user1_id}/{user2_id}/{count}");

    }

}
