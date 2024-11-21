using Keepi.Shared;

namespace Keepi.Client.Repositories.Interfaces
{
    public interface INotificationsRepository
    {
        Task<List<NotificationsData>> ReadFile(string userId);

        Task<List<bool>> CheckForNewNotifications(string userId);
    }
}
