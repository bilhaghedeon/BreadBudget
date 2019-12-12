using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BreadBudget.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;

namespace BreadBudget.Controllers
{
    public class HomeController : Controller

    {
        private readonly UserDb _context;

        private static int _currentUserId { get; set; }

        private static string _currentCategory { get; set; }

        public HomeController(UserDb context)
        {
            _context = context;
        }

        /* -------------------- Login -------------------- */

        // Login page
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LogIn login)
        {
            if (ModelState.IsValid)
            {
                var queryEmail = _context.Accounts.Any(a => a.Email == login.Email);
                if (queryEmail == true)
                {
                    var queryAccount = _context.Accounts.Any(a => a.Email == login.Email && a.Password == login.Password);
                    if (queryAccount == true)
                    {
                        _currentUserId = _context.Accounts.FirstOrDefault(u => u.Email == login.Email).Id;
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("InvalidPassword", "Invalid password. Please try again.");
                        return View();
                    }
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("AccountNotFound", "Invalid login. Please try again.");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public Boolean ValidateEmail(string email)
        {

            if (_context.Accounts.Any(a => a.Email == email))
            {
                return false;
            }
            return true;
        }

        /* -------------------- Sign up -------------------- */

        [HttpPost]
        public IActionResult SignUp(SignUp model)
        {
            if (ModelState.IsValid)
            {
                if (ValidateEmail(model.Email))
                {

                    string fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
                    string extension = Path.GetExtension(model.ProfilePicture.FileName);
                    fileName = Guid.NewGuid().ToString() + "_" + fileName + extension;
                    string filePath = Path.Combine("wwwroot/images/", fileName);
                    model.ProfilePicture.CopyTo(new FileStream(filePath, FileMode.Create));

                    Account newAccount = new Account(model.Name, model.Email, model.Password, fileName);
                    _context.Add(newAccount);
                    _context.SaveChanges();
                    _currentUserId = newAccount.Id;
                }
                else
                {
                    ModelState.AddModelError("Email", "Account with this email already exists.");
                    return View();
                }
                return RedirectToAction("AddTransaction");
            }
            else
            {
                return View();
            }
        }

        /* -------------------- After user logged in -------------------- */

        public IActionResult Dashboard()
        {
            if (_currentUserId == 0)
            {
                return View("Errors");
            }
            Account account = _context.Accounts.SingleOrDefault(a => a.Id == _currentUserId);

            return View(account);
        }

        /* -------------------- Transaction related functionalities  -------------------- */

        [HttpGet]
        public ViewResult AddTransaction()
        {
            if (_currentUserId == 0)
            {
                return View("Errors");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddTransaction(TransactionForm form)
        {
            if (ModelState.IsValid)
            {
                Account account = _context.Accounts.SingleOrDefault(a => a.Id == _currentUserId);
                List<Transaction> transactions = account.Transactions;

                string fileName = "";
                if (form.Receipt != null)
                {
                    fileName = Path.GetFileNameWithoutExtension(form.Receipt.FileName);
                    string extension = Path.GetExtension(form.Receipt.FileName);
                    fileName = Guid.NewGuid().ToString() + "_" + fileName + extension;
                    string filePath = Path.Combine("wwwroot/images/", fileName);
                    form.Receipt.CopyTo(new FileStream(filePath, FileMode.Create)); 
                }
                /*else {
                    fileName = "noReceipt.png";
                    string filePath = Path.Combine("wwwroot/images/", fileName);
                }*/
                Transaction newTransaction = new Transaction(form.TransactionType, form.Name, form.Amount ?? 0, form.Category, form.Note, fileName);

                account.Transactions.Add(newTransaction);
                _context.Accounts.Attach(account);
                var entry = _context.Entry(account);
                entry.Collection(a => a.Transactions).IsModified = true;
                _context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            else
            {
                return View();
            }
        }
        public IActionResult AllCategories()
        {
            List<string> categories = new List<string>()
            {
                "Housing", "Grocery", "Transportation", "Clothes", "Bills", "Food", "Health", "Miscellaneous", "Income"
            };
            if (_currentUserId == 0)
            {
                return View("Errors");
            }
            return View(categories);
        }

        public async Task<IActionResult> TransactionByCategory(string category)
        {
            if (_currentUserId == 0)
            {
                return View("Errors");
            }

            _currentCategory = category;

            var account = await _context
                .Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == _currentUserId);
            var transactions = account.Transactions.Where(t => t.Category == category).ToList();

            return View(transactions);
        }

        public async Task<IActionResult> AllTransactions()
        {
            if (_currentUserId == 0)
            {
                return View("Errors");
            }

            var account = await _context
               .Accounts
               .Include(a => a.Transactions)
               .FirstOrDefaultAsync(a => a.Id == _currentUserId);


            return View(account.Transactions.ToList());

        }

        public async Task<IActionResult> Delete(int id)
        {

            var account = await _context
              .Accounts
              .Include(a => a.Transactions)
              .FirstOrDefaultAsync(a => a.Id == _currentUserId);

            Transaction transaction = account.Transactions.Where(t => t.Id == id).FirstOrDefault();

            if (account == null)
            {
                return View("Errors");
            }

            _context.Entry(transaction).State = EntityState.Deleted;
            account.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return View("TransactionByCategory", account.Transactions.Where(t => t.Category == _currentCategory).ToList());

        }

        /* -------------------- Visualization of data using Chart JS  -------------------- */

        public async Task<IActionResult> Breakdown()
        {
            if (_currentUserId == 0) {
                return View("Errors");
            }

            //Account account = await _context.Accounts.FindAsync(_currentUserId);

            var account = await _context
                .Accounts
                .Include(s => s.Transactions)
                .FirstOrDefaultAsync(s => s.Id == _currentUserId);

            if (account == null)
            {
                return NotFound();
            }

            var lstModel = new List<BreakdownViewModel>();

            Dictionary<string, double> categoryTotalAmounts = new Dictionary<string, double>();
            IEnumerable<string> categories = account.Transactions.Select(s => s.Category).Distinct().ToList();

            double totalQuantity = 0;

            foreach (var t in account.Transactions)
            {

                totalQuantity += t.Amount;

                string category = Enum.GetName(typeof(TransactionForm.Categories), Int32.Parse(t.Category));

                if (categoryTotalAmounts.ContainsKey(category))
                {
                    categoryTotalAmounts[category] = categoryTotalAmounts[category] + t.Amount;
                }
                else
                {
                    categoryTotalAmounts.Add(category, t.Amount);
                }

            }

            foreach (var c in categories)
            {
                
                string category = Enum.GetName(typeof(TransactionForm.Categories), Int32.Parse(c));
                if (category != "Income")
                {
                    lstModel.Add(new BreakdownViewModel
                    {
                        Category = category,
                        TotalAmount = (int)Math.Round((categoryTotalAmounts[category]))

                    });
                }
            }

            IEnumerable<BreakdownViewModel> categoriesIEnum = lstModel;
            return View(categoriesIEnum);
        }

        /* -------------------- News API  -------------------- */

        public async Task<IActionResult> News()
        {
            if (_currentUserId == 0)
            {
                return View("Errors");
            }

            var newsApiClient = new NewsApiClient("465ab82acc5b43999381d54823215a61");
            var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = "Budget, Budgeting, Finance",
                SortBy = SortBys.Relevancy,
                Language = Languages.EN
            });


            return View("News", articlesResponse);

        }

        /* -------------------- User logs out  -------------------- */

        public IActionResult SignOut()
        {
            _currentUserId = 0;
            return RedirectToAction("Index");
        }

      


    }
}
