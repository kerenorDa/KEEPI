using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Keepi.Shared;
using System.Security.Cryptography;

namespace Keepi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly Db_Context _context;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(Db_Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //[HttpPost]
        //public async Task<User> UploadImage([FromBody] ImageUploadModel model)
        //{
        //    if (model == null || string.IsNullOrEmpty(model.ImageBase64))
        //    {
        //        return null;
        //    }

        //    var base64Data = model.ImageBase64.Substring(model.ImageBase64.IndexOf(",") + 1);
        //    var imageBytes = Convert.FromBase64String(base64Data);

        //    var directory = Path.Combine(Directory.GetCurrentDirectory(), "User Profiles");
        //    var filePath = Path.Combine(directory, $"{model.UserId}.png");

        //    if (System.IO.File.Exists(filePath))
        //    {
        //        System.IO.File.Delete(filePath);
        //    }

        //    await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
        //    var user = await _context.Users.FindAsync(model.UserId);
        //    if (user != null)
        //    {
        //        user.ProfilePhoto = Path.Combine("User Profiles", $"{model.UserId}.png");
        //        await _context.SaveChangesAsync();
        //        return user;
        //    }
        //    //var relativePath = $"/images/{Path.GetFileName(filePath)}";
        //    return null;
        //}

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromBody] ImageUploadModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.ImageBase64))
            {
                return BadRequest("Invalid image data");
            }

            var base64Data = model.ImageBase64.Substring(model.ImageBase64.IndexOf(",") + 1);
            var imageBytes = Convert.FromBase64String(base64Data);

            DateTime dateTime = DateTime.Now;
            string name = model.UserId.ToString() + ";" + dateTime.ToString("dd_MM_yyyy_HH_mm");
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "User Profiles");
            //var filePath = Path.Combine(directory, $"{model.UserId}.png");
            var filePath = Path.Combine(directory, $"{name}.png");


            string[] files = Directory.GetFiles(directory);
            foreach (string f in files)
            {
                var n = Path.GetFileName(f);
                if (n.Contains(model.UserId.ToString()))
                {
                    System.IO.File.Delete(f);
                }
            }

            //if (System.IO.File.Exists(filePath))
            //{
            //    System.IO.File.Delete(filePath);
            //}

            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
            var user = await _context.Users.FindAsync(model.UserId);
            if (user != null)
            {
                //user.ProfilePhoto = Path.Combine("User Profiles", $"{model.UserId}.png");
                user.ProfilePhoto = Path.Combine("User Profiles", $"{name}.png");
                await _context.SaveChangesAsync();
            }
            //var relativePath = $"/images/{Path.GetFileName(filePath)}";
            return Ok(user);
        }

        [HttpGet("editFirstName/{userId}/{newFirstName}")]
        public async Task<List<User>> EditFirstName(Guid userId, string newFirstName)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    user.FirstName = newFirstName;
                    await _context.SaveChangesAsync();
                    return new List<User> { user };
                }
            }
            return new List<User> { };
        }

        [HttpGet("editLastName/{userId}/{newLastName}")]
        public async Task<List<User>> EditLastName(Guid userId, string newLastName)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    user.LastName = newLastName;
                    await _context.SaveChangesAsync();
                    return new List<User> { user };
                }
            }
            return new List<User> { };
        }

        [HttpGet("editPassword/{userId}/{newPassword}")]
        public async Task<List<User>> EditPassword(Guid userId, string newPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

                    user.Password = hashedPassword;
                    await _context.SaveChangesAsync();
                    return new List<User> { user };
                }
            }
            return new List<User> { };
        }

        [HttpGet("editCity/{userId}/{newCity}")]
        public async Task<List<User>> EditCity(Guid userId, string newCity)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    user.City = newCity;
                    await _context.SaveChangesAsync();
                    return new List<User> { user };
                }
            }
            return new List<User> { };
        }

        [HttpGet("editPhoneNumber/{userId}/{newPhoneNumber}")]
        public async Task<List<User>> EditPhoneNumber(Guid userId, string newPhoneNumber)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    user.PhoneNumber = newPhoneNumber;
                    await _context.SaveChangesAsync();
                    return new List<User> { user };
                }
            }
            return new List<User> { };
        }

        [HttpGet("editAge/{userId}/{newAge}")]
        public async Task<List<User>> EditAge(Guid userId, int newAge)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    user.Age = newAge;
                    await _context.SaveChangesAsync();
                    return new List<User> { user };
                }
            }
            return new List<User> { };
        }

        [HttpGet("followUser/{_CurrentUserId}/{_UserIdToFollow}")]
        public async Task<List<User>> FollowUser(Guid _CurrentUserId, Guid _UserIdToFollow)
        {
            try
            {
                User CurrentUser = _context.Users.FirstOrDefault(u => u.Id == _CurrentUserId);
                User UserToFollow = _context.Users.FirstOrDefault(u => u.Id == _UserIdToFollow);

                if (CurrentUser != null && UserToFollow != null)
                {
                    CurrentUser.Following += _UserIdToFollow + ";";
                    UserToFollow.Followers += _CurrentUserId + ";";
                    await _context.SaveChangesAsync();


                    var a = await NotificationsHelper.AddNewNotification(UserToFollow.Id.ToString(), NotificationType.Follower, $"{CurrentUser.Username} started following you");

                    return new List<User> { CurrentUser };
                }

            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        [HttpGet("unFollowUser/{_CurrentUserId}/{_UserIdToUnFollow}")]
        public async Task<List<User>> UnFollowUser(Guid _CurrentUserId, Guid _UserIdToUnFollow)
        {
            try
            {
                User CurrentUser = _context.Users.FirstOrDefault(u => u.Id == _CurrentUserId);
                User UserToUnFollow = _context.Users.FirstOrDefault(u => u.Id == _UserIdToUnFollow);

                if (CurrentUser != null && UserToUnFollow != null)
                {
                    string[] currentUser_followingList = CurrentUser.Following.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var filteredFollowingList = currentUser_followingList.Where(item => item != _UserIdToUnFollow.ToString());
                    CurrentUser.Following = string.Join(";", filteredFollowingList) + ";";

                    string[] userToUnfollow_followersList = UserToUnFollow.Followers.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var filteredFollowersList = userToUnfollow_followersList.Where(item => item != _CurrentUserId.ToString());
                    UserToUnFollow.Followers = string.Join(";", filteredFollowersList) + ";";


                    await _context.SaveChangesAsync();

                    return new List<User> { CurrentUser };
                }

            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        [HttpGet("getFollowers/{userId}")]
        public async Task<List<User>> GetUserFollowersList(Guid userId)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Id == userId);
                List<User> followers = new List<User>();

                if (user != null)
                {
                    string[] followers_id = user.Followers.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (followers_id.Length > 0)
                    {
                        foreach (var _id in followers_id)
                        {
                            User follower = await _context.Users.FindAsync(Guid.Parse(_id));
                            if (follower != null)
                            {
                                followers.Add(follower);
                            }
                        }
                    }

                    return followers;
                }

            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        [HttpGet("getFollowing/{userId}")]
        public async Task<List<User>> GetUserFollowingList(Guid userId)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Id == userId);
                List<User> following = new List<User>();

                if (user != null)
                {
                    string[] following_id = user.Following.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (following_id.Length > 0)
                    {
                        foreach (var _id in following_id)
                        {
                            User _following = await _context.Users.FindAsync(Guid.Parse(_id));
                            if (_following != null)
                            {
                                following.Add(_following);
                            }
                        }
                    }

                    return following;
                }

            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        [HttpGet("getProfilePath/{userId}")]
        public async Task<List<string>> GetUserProfileImagePath(Guid userId)
        {
            try
            {
                var filePath = Path.Combine("User Profiles", $"{userId}.png");
                var directory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                if (System.IO.File.Exists(directory))
                {
                    return new List<string> { filePath };
                }
                return new List<string>();
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        [HttpGet("getUser/{userId}")]
        public async Task<List<User>> GetUser(Guid userId)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Id == userId);
               
                if (user != null)
                {
                    return new List<User> { user };
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

    }

}
