using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreadBudget.Models
{
    public class SignUp
    {

        // first page ask for

        public string Name { get; set; }

        public string Email { get; set;  }

        public string Password { get; set; }

        public string ProfilePicture { get; set; }

        /* second page ask for general information to start budget */

        // How much do you have saved? 
        public double Savings { get; set; }

        /* Transactions to get user started with budget */

        // How much income are you making in a month?

        public double monthlyIncome { get; set; }

        // Log an expense: Food, Rent, Miscellaneous

        public bool expenseChosen { get; set; }

        public double expenseAmount { get; set; }



 
       

        





    }
}
