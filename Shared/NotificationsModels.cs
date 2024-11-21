using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keepi.Shared
{
    public class NotificationsData
    {
        public string FileId { get; set; }
        public bool IsNewNotifications { get; set; }
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }

    public class Notification
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public enum NotificationType
    {
        Follower, Message, Post, Wallet
    }
}
