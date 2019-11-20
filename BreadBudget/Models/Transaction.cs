using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreadBudget.Models
{
    public class Transaction
    {
        public string TransactionType { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }
        public int Id { get; set; } 
        
       

        public Transaction(string transactionType, string name, double amount, string category, string note, int id)
        {
            TransactionType = transactionType;
            Name = name;
            Amount = amount;
            Category = category;
            Note = note;
            Id = id;

        }
        

    }
}
