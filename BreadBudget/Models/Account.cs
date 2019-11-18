using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreadBudget.Models
{
    public class Account
    {
       
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ProfilePicture { get; set; }

        public Account(string name, string email, string password, string profilePicture)
        {
            Name = name;
            Email = email;
            Password = password;
            ProfilePicture = profilePicture;
        }


    }
}
