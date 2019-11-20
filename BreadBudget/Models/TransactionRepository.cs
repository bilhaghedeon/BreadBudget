using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreadBudget.Models
{
    public class TransactionRepository
    {
        public static int id = 1;

        private static List<TransactionForm> forms = new List<TransactionForm>() ;
        public static void AddForm(TransactionForm form)
        {
            id++;
            form.Id = id;

            forms.Add(form);

        }

        public static IEnumerable<TransactionForm> Forms
        {
            get
            {
                return forms;
            }
        }
    }
}
