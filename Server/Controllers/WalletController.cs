using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Keepi.Shared;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Keepi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly Db_Context _context;

        public WalletController(Db_Context context)
        {
            _context = context;
        }

        [HttpGet("withdrawal/{userId}/{count}")]
        public async Task<List<int>> Withdrawal(Guid userId, int count)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                    if (user != null)
                    {
                        if (user.WalletCount == 0)
                        {
                            return new List<int> { };
                        }

                        user.WalletCount -= count;
                        await _context.SaveChangesAsync();

                        return new List<int> { user.WalletCount };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }

        [HttpGet("deposit/{userId}/{count}")]
        public async Task<List<int>> Deposit(Guid userId, int count)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                    if (user != null)
                    {
                        user.WalletCount += count;
                        await _context.SaveChangesAsync();

                        return new List<int> { user.WalletCount };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }

        [HttpGet("depositToOtherUser/{user1_id}/{user2_id}/{count}")]
        public async Task<List<bool>> DepositToOtherUser(Guid user1_id, Guid user2_id, int count)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user1 = await _context.Users.FirstOrDefaultAsync(u => u.Id == user1_id);
                    User user2 = await _context.Users.FirstOrDefaultAsync(u => u.Id == user2_id);

                    if (user1 != null && user2 != null)
                    {
                        if (user1.WalletCount < count)
                        {
                            return new List<bool> { false };
                        }

                        user1.WalletCount -= count;
                        user2.WalletCount += count;

                        await _context.SaveChangesAsync();

                        var a = await NotificationsHelper.AddNewNotification(user2.Id.ToString(), NotificationType.Wallet, $"{user1.Username} deposited {count} coins into your wallet");

                        return new List<bool> { true };
                    }
                    else
                    {
                        return null;
                    }
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
