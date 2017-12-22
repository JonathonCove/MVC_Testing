using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutomatedTeller.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }

        public int CheckingAccountId { get; set; }
        public virtual CheckingAccount CheckingAccount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

    }
}