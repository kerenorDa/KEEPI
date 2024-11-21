using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Keepi.Shared;
using System.IO;
using System.Text.Json;

namespace Keepi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly Db_Context _context;
        private readonly string chatFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Chat Files");

        public ChatController(Db_Context context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] string[] model)
        {
            string fileName = model[0];
            string userId = model[1];
            string message = model[2];
            
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(message))
            {
                return BadRequest("Invalid data");
            }

            try
            {
                var chatFilePath = Path.Combine(chatFilesDirectory, $"{fileName}.json");

                if (System.IO.File.Exists(chatFilePath))
                {
                    var chatData = JsonSerializer.Deserialize<ChatData>(System.IO.File.ReadAllText(chatFilePath));

                    //string[] users = fileName.Split(';');
                    User sender = null;
                    User receiver = null;

                    if (chatData != null)
                    {
                        Console.WriteLine("1" + chatData.ChatId);
                        //if (users[0] == userId)
                        if (userId == chatData.User1_Id)
                        {
                            sender = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == chatData.User1_Id);
                            receiver = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == chatData.User2_Id);

                            chatData.IsNewMessages_user2 = true;
                        }
                        //else if (users[1] == userId)
                        else if (userId == chatData.User2_Id)
                        {
                            sender = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == chatData.User2_Id);
                            receiver = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == chatData.User1_Id);

                            chatData.IsNewMessages_user1 = true;
                        }

                        chatData.Messages.Add(new ChatMessage
                        {
                            UserId = userId,
                            Content = message,
                            Timestamp = DateTime.UtcNow
                        });

                        var updatedJsonContent = JsonSerializer.Serialize(chatData, new JsonSerializerOptions { WriteIndented = true });
                        System.IO.File.WriteAllText(chatFilePath, updatedJsonContent);
                        Console.WriteLine("2" + chatData.ChatId);

                        if (sender != null && receiver != null)
                        {
                            var a = NotificationsHelper.AddNewNotification(receiver.Id.ToString(), NotificationType.Message, $"{sender.Username} sent you a message");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(true);
        }

        [HttpGet("createNewChat/{user1}/{user2}")]
        public async Task<List<Chat>> CreateNewChatFile(string user1, string user2)
        {
            try
            {
                if (!Directory.Exists(chatFilesDirectory))
                {
                    Directory.CreateDirectory(chatFilesDirectory);
                }

                User u = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == user2);

                string fileName_option1 = user1 + ";" + user2;
                string fileName_option2 = user2 + ";" + user1;

                var chatFilePath_option1 = Path.Combine(chatFilesDirectory, $"{fileName_option1}.json");
                var chatFilePath_option2 = Path.Combine(chatFilesDirectory, $"{fileName_option2}.json");

                if (System.IO.File.Exists(chatFilePath_option1))
                {
                    return new List<Chat> { new Chat() { FileName = fileName_option1, User = u } };
                }
                else if (System.IO.File.Exists(chatFilePath_option2))
                {
                    return new List<Chat> { new Chat() { FileName = fileName_option2, User = u } };
                }
                else
                {
                    var initialChatData = new
                    {
                        ChatId = fileName_option1,
                        User1_Id = user1,
                        User2_Id = user2,
                        IsNewMessages_user1 = false,
                        IsNewMessages_user2 = false,
                        Messages = new List<object>()
                    };

                    var jsonContent = JsonSerializer.Serialize(initialChatData, new JsonSerializerOptions { WriteIndented = true });
                    System.IO.File.WriteAllText(chatFilePath_option1, jsonContent);

                    return new List<Chat> { new Chat() { FileName = fileName_option1, User = u } };
                }

                //if (!System.IO.File.Exists(chatFilePath))
                //{
                //    var initialChatData = new
                //    {
                //        chatId = fileName,
                //        messages = new List<object>()
                //    };

                //    var jsonContent = JsonSerializer.Serialize(initialChatData, new JsonSerializerOptions { WriteIndented = true });
                //    System.IO.File.WriteAllText(chatFilePath, jsonContent);
                //}
            }
            catch (Exception)
            {
                return null;
            }


            return new List<Chat> { };
        }

        [HttpGet("addMessageToChat/{fileName}/{userId}/{message}")]
        public async Task<List<bool>> AddMessageToChat(string fileName, string userId, string message)
        {
            try
            {
                var chatFilePath = Path.Combine(chatFilesDirectory, $"{fileName}.json");

                if (System.IO.File.Exists(chatFilePath))
                {
                    var chatData = JsonSerializer.Deserialize<ChatData>(System.IO.File.ReadAllText(chatFilePath));

                    //string[] users = fileName.Split(';');
                    User sender = null;
                    User receiver = null;

                    if (chatData != null)
                    {
                        //if (users[0] == userId)
                        if (userId == chatData.User1_Id)
                        {
                            sender = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == chatData.User1_Id);
                            receiver = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == chatData.User2_Id);

                            chatData.IsNewMessages_user2 = true;
                        }
                        //else if (users[1] == userId)
                        else if (userId == chatData.User2_Id)
                        {
                            sender = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == chatData.User2_Id);
                            receiver = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == chatData.User1_Id);

                            chatData.IsNewMessages_user1 = true;
                        }

                        chatData.Messages.Add(new ChatMessage
                        {
                            UserId = userId,
                            Content = message,
                            Timestamp = DateTime.UtcNow
                        });

                        var updatedJsonContent = JsonSerializer.Serialize(chatData, new JsonSerializerOptions { WriteIndented = true });
                        System.IO.File.WriteAllText(chatFilePath, updatedJsonContent);

                        if (sender != null && receiver != null)
                        {
                            var a = NotificationsHelper.AddNewNotification(receiver.Id.ToString(), NotificationType.Message, $"{sender.Username} sent you a message");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return new List<bool> { true };
        }

        [HttpGet("readChatFile/{fileName}/{userId}")]
        public async Task<List<ChatData>> ReadChatFile(string fileName, string userId)
        {
            var chatFilePath = Path.Combine(chatFilesDirectory, $"{fileName}.json");
           
            if (System.IO.File.Exists(chatFilePath))
            {
                var chatData = JsonSerializer.Deserialize<ChatData>(System.IO.File.ReadAllText(chatFilePath));
                if (chatData != null)
                {
                    //string[] users = fileName.Split(';');

                    if (userId == chatData.User1_Id)
                    {
                        if (chatData.IsNewMessages_user1)
                        {
                            chatData.IsNewMessages_user1 = false;
                            var updatedJsonContent = JsonSerializer.Serialize(chatData, new JsonSerializerOptions { WriteIndented = true });
                            System.IO.File.WriteAllText(chatFilePath, updatedJsonContent);
                        }
                    }
                    else if (userId == chatData.User2_Id)
                    {
                        if (chatData.IsNewMessages_user2)
                        {
                            chatData.IsNewMessages_user2 = false;
                            var updatedJsonContent = JsonSerializer.Serialize(chatData, new JsonSerializerOptions { WriteIndented = true });
                            System.IO.File.WriteAllText(chatFilePath, updatedJsonContent);
                        }
                    }
                    return new List<ChatData> { chatData };
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        [HttpGet("getUserChats/{user}")]
        public async Task<List<Chat>> GetUserChats(string user)
        {
            List<Chat> chats = new List<Chat>();

            try
            {
                if (Directory.Exists(chatFilesDirectory))
                {
                    string[] files = Directory.GetFiles(chatFilesDirectory);

                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        string[] tmp = fileName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tmp.Length > 0)
                        {
                            if (tmp[0] == user)
                            {
                                User u = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == tmp[1]);
                                if (u != null)
                                {
                                    Chat c = new Chat() { User = u, FileName = fileName };

                                    var chatFilePath = Path.Combine(chatFilesDirectory, $"{fileName}.json");
                                    if (System.IO.File.Exists(chatFilePath))
                                    {
                                        var chatData = JsonSerializer.Deserialize<ChatData>(System.IO.File.ReadAllText(chatFilePath));
                                        if (chatData != null)
                                            c.Data = chatData;
                                    }
                                    
                                    chats.Add(c);
                                }
                            }
                            else if (tmp[1] == user)
                            {
                                User u = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == tmp[0]);
                                if (u != null)
                                {
                                    Chat c = new Chat() { User = u, FileName = fileName };

                                    var chatFilePath = Path.Combine(chatFilesDirectory, $"{fileName}.json");
                                    if (System.IO.File.Exists(chatFilePath))
                                    {
                                        var chatData = JsonSerializer.Deserialize<ChatData>(System.IO.File.ReadAllText(chatFilePath));
                                        if (chatData != null)
                                            c.Data = chatData;
                                    }

                                    chats.Add(c);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return chats;
        }

        [HttpGet("deleteChat/{fileName}")]
        public async Task<List<bool>> DeleteChat(string fileName)
        {
            try
            {
                string filePath = Path.Combine(chatFilesDirectory, $"{fileName}.json");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return new List<bool> { true };
                }
            }
            catch (Exception)
            {
                return new List<bool> { false };
            }

            return new List<bool> { false };
        }
    }
}
