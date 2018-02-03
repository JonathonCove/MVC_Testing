using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scarecrow.ViewModels
{
    public class UserViewModels
    {
        public class Signup
        {
            [Required]
            [StringLength(50, ErrorMessage = "Your username cannot exceed 50 characters")]
            public string Username { get; set; }

            [Required]
            [Display(Name="Email Address")]
            [EmailAddress]
            [DataType(DataType.EmailAddress)]
            public string EmailAddress { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(250, ErrorMessage = "Your password cannot exceed 250 characters")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name ="Confirm Password")]
            [Compare("Password", ErrorMessage ="Your passwords didn't match")]
            public string ConfirmPassword { get; set; }
        }

        public class Login
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name ="Email Address")]
            public string EmailAddress { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}