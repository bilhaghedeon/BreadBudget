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
        public enum Categories { Housing, Groceries, Transportation, Clothes, Bills, Food, Health, Miscellaneous, Income }

        [Required(ErrorMessage = "Please select your transaction type.")]
        public string TransactionType { get; set; }
        [Required(ErrorMessage = "Please name your transaction.")]
        [StringLength(100)]
        public string Name { get; set; }

        
        [RegularExpression(@"^\d{0,8}(\.\d{1,4})?$",
            ErrorMessage = "Please enter a valid amount.")]
        [Required(ErrorMessage = "Please enter an amount.")]
        public double? Amount { get; set; }
       

        [Required(ErrorMessage = "Please choose a Category type")]
        [EnumDataType(typeof(Categories))]
        public string Category { get; set; }

        
        [StringLength(50, ErrorMessage ="Please enter a shorter note.")]
        public string Note { get; set; }

        public int Id { get; set; }

        public IFormFile Receipt { get; set; }


    }
}
