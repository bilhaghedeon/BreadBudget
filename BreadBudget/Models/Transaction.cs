using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreadBudget.Models
{
    public class Transaction
    {
        enum TransactionType { Expense, Income }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }
        public int Id { get; set; }

    }
}
