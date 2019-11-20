using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BreadBudget.Models
{
    public class SignUp
    {

        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        public string Email { get; set;  }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        public IFormFile ProfilePicture { get; set; }




 
       

        





    }
}
