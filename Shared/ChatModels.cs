using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keepi.Shared
{
    public class Chat
    {
        public string FileName { get; set; }
        public User User { get; set; }
        public ChatData Data { get; set; }
    }
    public class ChatData
    {
        public string ChatId { get; set; }
        public string User1_Id { get; set; }
        public string User2_Id { get; set; }
        public bool IsNewMessages_user1 { get; set; }
        public bool IsNewMessages_user2 { get; set; }
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }

    public class ChatMessage
    {
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
