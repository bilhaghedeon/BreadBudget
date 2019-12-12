/* Author: Everyone */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BreadBudget.Models
{
    public class UserDb : DbContext
    {
        public UserDb(DbContextOptions<UserDb> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
    }
}
