using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BreadBudget.Models
{
    public class UserDb : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public UserDb(DbContextOptions<UserDb> options) : base(options)
        {

        }
    }
}
