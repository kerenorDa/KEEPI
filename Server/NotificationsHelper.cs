using Keepi.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Keepi.Server
{
    public static class NotificationsHelper
    {
        private static readonly string FilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Notifications Files");


        public static async Task<bool> CreateNewFile(string userId)
        {
            try
            {
                if (!Directory.Exists(FilesDirectory))
                {
                    Directory.CreateDirectory(FilesDirectory);
                }

                var filePath = Path.Combine(FilesDirectory, $"{userId}.json");

                var initialData = new
                {
                    FileId = userId,
                    IsNewNotification = false,
                    Notifications = new List<object>()
                };

                var jsonContent = JsonSerializer.Serialize(initialData, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(filePath, jsonContent);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> AddNewNotification(string userId, NotificationType notificationType, string notification)
        {
            try
            {
                var filePath = Path.Combine(FilesDirectory, $"{userId}.json");

                if (System.IO.File.Exists(filePath))
                {
                    var data = JsonSerializer.Deserialize<NotificationsData>(System.IO.File.ReadAllText(filePath));

                    if (data != null)
                    {
                        data.IsNewNotifications = true;
                        data.Notifications.Add(new Notification
                        {
                            Type = notificationType.ToString(),
                            Content = notification,
                            Timestamp = DateTime.UtcNow
                        });

                        var updatedJsonContent = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                        System.IO.File.WriteAllText(filePath, updatedJsonContent);
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public static async Task<NotificationsData> ReadFile(string userId)
        {
            var filePath = Path.Combine(FilesDirectory, $"{userId}.json");

            if (System.IO.File.Exists(filePath))
            {
                var data = JsonSerializer.Deserialize<NotificationsData>(System.IO.File.ReadAllText(filePath));
                if (data != null)
                {
                    data.IsNewNotifications = false;
                    var updatedJsonContent = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                    System.IO.File.WriteAllText(filePath, updatedJsonContent); 
                    return data;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        public static async Task<bool> CheckForNewNotifications(string userId)
        {
            var filePath = Path.Combine(FilesDirectory, $"{userId}.json");

            if (System.IO.File.Exists(filePath))
            {
                var data = JsonSerializer.Deserialize<NotificationsData>(System.IO.File.ReadAllText(filePath));
                if (data != null)
                {
                    return data.IsNewNotifications;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
