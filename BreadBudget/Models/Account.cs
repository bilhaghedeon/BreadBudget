using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BreadBudget.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string ProfilePicture { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public Account(string name, string email, string password, string profilePicture)
        {
            Name = name;
            Email = email;
            Password = password;
            ProfilePicture = profilePicture;
        }


        public Account(string email, string password)
        {
            Email = email;
            Password = password;
        }


    }
}
