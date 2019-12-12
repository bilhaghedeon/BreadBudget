using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BreadBudget.Models
{
    public class TransactionForm
    {
        public enum TransactionTypes { Expense, Income }
        public enum Categories { Housing, Groceries, Transportation, Clothes, Bills, Food, Health, Miscellaneous}

        [Required(ErrorMessage = "Please select your transaction type")]
        public string TransactionType { get; set; }
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(100 )]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your an amount.")]
        [RegularExpression(@"^\d{0,8}(\.\d{1,2})?$",
            ErrorMessage = "Please enter a valid amount.")]
        public double? Amount { get; set; }

        [Required(ErrorMessage = "Please choose a device type")]
        [EnumDataType(typeof(Categories))]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(300)]
        public string Note { get; set; }

        public int Id { get; set; }

        public IFormFile Receipt { get; set; }


    }
}
