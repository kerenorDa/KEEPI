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
    public class RegisterController : ControllerBase
    {
        private readonly Db_Context _context;

        public RegisterController(Db_Context context)
        {
            _context = context;
        }

        [HttpGet("_register/{_username}/{_firstName}/{_lastName}/{_password}/{_email}/{_city}/{_age}/{_phoneNumber}")]
        public async Task<List<User>> Register(string _username, string _firstName, string _lastName, string _password, string _email, string _city, int _age, string _phoneNumber)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == _username || u.Email == _email);
                    if (existingUser != null)
                    {
                        return new List<User> { };
                    }

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(_password);

                    var user = new User
                    {
                        Id = new Guid(),
                        Username = _username,
                        FirstName = _firstName,
                        LastName = _lastName,
                        Password = hashedPassword,
                        Email = _email,
                        City = _city,
                        Age = _age,
                        PhoneNumber = _phoneNumber,
                        ProfilePhoto = "User Profiles\\default.png"
                    };

                    //user.ProfilePhoto = "/User Profiles/" + user.Id.ToString() + ".png";

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    var b = await NotificationsHelper.CreateNewFile(user.Id.ToString());

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
