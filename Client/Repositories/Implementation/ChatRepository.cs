using Keepi.Client.Communication;
using Keepi.Client.Repositories.Base;
using Keepi.Client.Repositories.Interfaces;
using Keepi.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Keepi.Client.Repositories.Implementation
{
    public class ChatRepository : Repository, IChatRepository
    {
        private const string URL = "api/Chat";
        private readonly HttpClient _httpClient;

        public ChatRepository(HttpClient httpClient, IHttpService commService) : base(commService)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Chat>> CreateNewChatFile(string user1, string user2) =>
             await Get<Chat>($"api/Chat/createNewChat/{user1}/{user2}");

        public async Task<List<bool>> AddMessageToChat(string fileName, string userId, string message) =>
             await Get<bool>($"api/Chat/addMessageToChat/{fileName}/{userId}/{message}");
   
        public async Task<List<ChatData>> ReadChatFile(string fileName, string userId) =>
             await Get<ChatData>($"api/Chat/readChatFile/{fileName}/{userId}");
   
        public async Task<List<Chat>> GetUserChats(string user) =>
             await Get<Chat>($"api/Chat/getUserChats/{user}");

        public async Task<List<bool>> DeleteChat(string fileName) =>
            await Get<bool>($"api/Chat/deleteChat/{fileName}");
    }

}
