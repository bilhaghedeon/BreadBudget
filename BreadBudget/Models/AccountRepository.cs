using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreadBudget.Models
{
    public class AccountRepository
    {

        private static List<Account> _accounts = new List<Account>();

        public static void AddAccount(Account newAccount)
        {
            _accounts.Add(newAccount);
        }



    }
}
