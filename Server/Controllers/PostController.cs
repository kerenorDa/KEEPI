using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Keepi.Shared;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Keepi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly Db_Context _context;

        public PostController(Db_Context context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> NewPost([FromBody] string[] model)
        {
            try
            {
                string Category = model[0];
                string Text = model[1];
                Guid UserId = Guid.Parse(model[2]);

                if (string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(Text) || string.IsNullOrEmpty(model[2]))
                {
                    return BadRequest("Invalid data");
                }

                var post = new Post
                {
                    Id = new Guid(),
                    Date = DateTime.Now,
                    Category = Category,
                    Content = Text,
                    UserId = UserId
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                var user = await _context.Users.FindAsync(UserId);
                if (user != null)
                {
                    string[] followers_id = user.Followers.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (followers_id.Length > 0)
                    {
                        foreach (var _id in followers_id)
                        {
                            var a = await NotificationsHelper.AddNewNotification(_id, NotificationType.Post, $"{user.Username} posted a new post");
                        }
                    }
                }

                return Ok(post);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("newPost/{Category}/{Text}/{UserId}")]
        public async Task<List<Post>> AddNewPost(string Category, string Text, Guid UserId)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Id = new Guid(),
                    Date = DateTime.Now,
                    Category = Category,
                    Content = Text,
                    UserId = UserId
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                var user = await _context.Users.FindAsync(UserId);
                if (user != null)
                {
                    string[] followers_id = user.Followers.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (followers_id.Length > 0)
                    {
                        foreach (var _id in followers_id)
                        {
                            var a = await NotificationsHelper.AddNewNotification(_id, NotificationType.Post, $"{user.Username} posted a new post");
                        }
                    }
                }

                return new List<Post>() { post };

            }
            return new List<Post> { };
        }

        [HttpGet("getAllPosts")]
        public async Task<List<Post>> GetAllPosts()
        {
            try
            {
                List<Post> posts = _context.Posts.ToList();

                foreach (var post in posts)
                {
                    post.User = _context.Users.FirstOrDefault(u => u.Id == post.UserId);
                }

                return posts;
            }
            catch (Exception)
            {
                return new List<Post> { };
            }
        }


        [HttpGet("getUserPosts/{_UserId}")]
        public async Task<List<Post>> GetPostsByUserId(Guid _UserId)
        {
            try
            {
                List<Post> posts = _context.Posts.Where(p => p.UserId == _UserId).ToList();
                User user = _context.Users.FirstOrDefault(u => u.Id == _UserId);

                foreach (var post in posts)
                {
                    post.User = user;
                }

                return posts;
            }
            catch (Exception)
            {
                return new List<Post> { };
            }

        }

        [HttpGet("deletePost/{_PostId}")]
        public async Task<List<bool>> DeletePost(Guid _PostId)
        {
            try
            {
                List<Post> posts = _context.Posts.Where(p => p.Id == _PostId).ToList();

                if (posts != null && posts.Count > 0)
                {
                    _context.Posts.Remove(posts[0]);
                    await _context.SaveChangesAsync();

                    return new List<bool> { true };
                }
                
            }
            catch (Exception)
            {
                return new List<bool> { false };
            }

            return new List<bool> { false };
        }


        [HttpGet("editPostCategory/{_PostId}/{_newCategory}")]
        public async Task<List<Post>> EditPostCategory(Guid _PostId, string _newCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var post = await _context.Posts.FindAsync(_PostId);

                    if (post != null)
                    {
                        post.Category = _newCategory;
                        await _context.SaveChangesAsync();
                        return new List<Post> { post };
                    }
                }
                return new List<Post> { };
            }
            catch (Exception)
            {
                return new List<Post> { };
            }
        }

        [HttpGet("EditPostContent/{_PostId}/{_newContent}")]
        public async Task<List<Post>> EditPostContent(Guid _PostId, string _newContent)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var post = await _context.Posts.FindAsync(_PostId);

                    if (post != null)
                    {
                        post.Content = _newContent;
                        await _context.SaveChangesAsync();
                        return new List<Post> { post };
                    }
                }
                return new List<Post> { };
            }
            catch (Exception)
            {
                return new List<Post> { };
            }
        }


        [HttpGet("savePostToUserCollection/{_UserId}/{_PostId}")]
        public async Task<List<bool>> SavePostToUserCollection(Guid _UserId, Guid _PostId)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Id == _UserId);

                if (user != null)
                {
                    user.SavedPosts += _PostId + ";";
                    await _context.SaveChangesAsync();

                    return new List<bool> { true };
                }

            }
            catch (Exception)
            {
                return new List<bool> { false };
            }

            return new List<bool> { false };
        }
        

        [HttpGet("getUserSavedPosts/{_UserId}")]
        public async Task<List<Post>> GetUserSavedPosts(Guid _UserId)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Id == _UserId);
                List<Post> posts = new List<Post>();

                if (user != null)
                {
                    string[] posts_id = user.SavedPosts.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (posts_id.Length > 0)
                    {
                        foreach (var _id in posts_id)
                        {
                            Post post = await _context.Posts.FindAsync(Guid.Parse(_id));
                            if (post != null)
                            {
                                User post_user = _context.Users.FirstOrDefault(u => u.Id == post.UserId);
                                if (post_user != null) 
                                {
                                    post.User = post_user;
                                }

                                posts.Add(post);
                            }
                        }
                    }

                    return posts;
                }

            }
            catch (Exception)
            {
                return new List<Post> { };
            }

            return new List<Post> { };
        }


        [HttpGet("unsavePostFromUserCollection/{_UserId}/{_PostId}")]
        public async Task<List<bool>> UnsavePostFromUserCollection(Guid _UserId, Guid _PostId)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Id == _UserId);

                if (user != null)
                {
                    string[] posts_id = user.SavedPosts.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var filteredPosts = posts_id.Where(item => item != _PostId.ToString());
                    user.SavedPosts = string.Join(";", filteredPosts) + ";";
                    await _context.SaveChangesAsync();

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
