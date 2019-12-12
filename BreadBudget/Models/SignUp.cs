using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BreadBudget.Models
{
    public class SignUp
    {

        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessage = "Please make sure you enter a valid e-mail address.")]
        public string Email { get; set;  }

        [Required(ErrorMessage = "Please enter your password.")]
        [RegularExpression(@".{8,}",
            ErrorMessage = "Please make sure password is at least 8 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        public IFormFile ProfilePicture { get; set; }




 
       

        





    }
}
