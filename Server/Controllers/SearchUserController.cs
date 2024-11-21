using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Keepi.Shared;
using static Duende.IdentityServer.Models.IdentityResources;

namespace Keepi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchUserController : ControllerBase
    {
        private readonly Db_Context _context;

        public SearchUserController(Db_Context context)
        {
            _context = context;
        }

        [HttpGet("getUserByUserName/{userName}")]
        public async Task<List<User>> GetUserByUserName(string userName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
                    if (user == null)
                    {
                        return new List<User> { };
                    }
                    return new List<User> { user };
                }
            }
            catch (Exception)
            {
                return new List<User> { };
            }

            return new List<User> { };
        }

        [HttpGet("searchUsers/{prefix}")]
        public async Task<List<string>> SearchUsers(string prefix)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var users = await _context.Users.Where(u => u.Username.StartsWith(prefix)).Select(u => u.Username).ToListAsync();
                    
                    return users;
                }
            }
            catch (Exception)
            {
                return new List<string> { };
            }

            return new List<string> { };
        }
    }

}
