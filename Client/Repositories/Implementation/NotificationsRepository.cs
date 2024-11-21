using Keepi.Client.Communication;
using Keepi.Client.Repositories.Base;
using Keepi.Client.Repositories.Interfaces;
using Keepi.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Keepi.Client.Repositories.Implementation
{
    public class NotificationsRepository : Repository, INotificationsRepository
    {
        private const string URL = "api/Notifications";
        private readonly HttpClient _httpClient;

        public NotificationsRepository(HttpClient httpClient, IHttpService commService) : base(commService)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NotificationsData>> ReadFile(string userId) =>
          await Get<NotificationsData>($"api/Notifications/readFile/{userId}");
    
        public async Task<List<bool>> CheckForNewNotifications(string userId) =>
          await Get<bool>($"api/Notifications/newNotifications/{userId}");
    }

}
