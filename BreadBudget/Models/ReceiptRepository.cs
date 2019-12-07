using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreadBudget.Models
{
    public static class ReceiptRepository
    {
        public static IEnumerable<Transaction> GetTransactions()
        {
            var transaction = new Transaction("hi","",10.00,"Rent","","");
            var transaction1 = new Transaction("hello","",10.00,"Groceries","","");
            var transaction2 = new Transaction("hey","",10.00,"Bills","","");
            return new List<Transaction>
            {
                transaction,
                transaction1,
                transaction2
            };
        }
    }
}
