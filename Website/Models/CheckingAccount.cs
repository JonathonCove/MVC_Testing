using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutomatedTeller.Models
{
    public class CheckingAccount
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"\d{6,10}", ErrorMessage="Account Number must be between 6 and 10 characters")]
        [Display(Name="Account No")]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public decimal Balance { get; set; }

        [Display(Name="Full Name")]
        public string FullName //needs to be on two lines for the display attribute
            => FirstName + " " + LastName;
    }
}