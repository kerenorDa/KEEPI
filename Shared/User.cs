using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keepi.Shared
{
    [Table("UsersTbl")]
    public class User
    {
        public User()
        {
                
        }

        public User(User user)
        {
            Id = user.Id;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Password = user.Password;
            Email = user.Email;
            City = user.City;
            PhoneNumber = user.PhoneNumber;
            Age = user.Age;
            ProfilePhoto = user.ProfilePhoto;
            Following = user.Following;
            Followers = user.Followers;
            WalletCount = user.WalletCount;
            SavedPosts = user.SavedPosts;
        }

      
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string ProfilePhoto { get; set; }
        public string Following { get; set; } = ";";
        public string Followers { get; set; } = ";";
        public int WalletCount { get; set; }
        public string SavedPosts { get; set; } = ";";


        //private string _profilePhoto;
        //public string ProfilePhoto
        //{
        //    get { return "/User Profiles/" + Id.ToString() + ".png"; }
        //    set { _profilePhoto = value; }
        //}

    }
}
