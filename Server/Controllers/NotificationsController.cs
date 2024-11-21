using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Keepi.Shared;

namespace Keepi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly Db_Context _context;

        public NotificationsController(Db_Context context)
        {
            _context = context;
        }

        [HttpGet("readFile/{userId}")]
        public async Task<List<NotificationsData>> ReadFile(string userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await NotificationsHelper.ReadFile(userId);
                    return new List<NotificationsData> { data };
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }

        [HttpGet("newNotifications/{userId}")]
        public async Task<List<bool>> CheckForNewNotifications(string userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var b = await NotificationsHelper.CheckForNewNotifications(userId);
                    return new List<bool> { b };
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }
    }
}
