using Keepi.Shared;

namespace Keepi.Client.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task<List<Chat>> CreateNewChatFile(string user1, string user2);
        Task<List<bool>> AddMessageToChat(string fileName, string userId, string message);
        Task<List<ChatData>> ReadChatFile(string fileName, string userId);
        Task<List<Chat>> GetUserChats(string user);
        Task<List<bool>> DeleteChat(string fileName);
    }
}
