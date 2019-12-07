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

namespace BreadBudget.Controllers
{
    public class HomeController : Controller

    {
        private UserDb _context;

        // current user account 
        private static int _currentUserId { get; set; }

        public HomeController(UserDb context)
        {
            _context = context;
        }

        // Index page deals with Log In functionality
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
                // Checks the database for the corresponding account email
                var queryEmail = _context.Accounts.Any(a => a.Email == login.Email);
                if (queryEmail == true)
                {
                    // Checks the database for the corresponding account password
                    var queryAccount = _context.Accounts.Any(a => a.Email == login.Email && a.Password == login.Password);

                    if (queryAccount == true)
                    {
                        
                        _currentUserId = _context.Accounts.FirstOrDefault(u => u.Email == login.Email).Id;

                       // TempData["Account"] = _currentUserId;
                        return View("Dashboard");
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

        public async Task<IActionResult> DisplayTest()
        {
            Account hello = _context.Accounts.Find(_currentUserId);

            var student = await _context
                .Accounts
                .Include(s => s.Transactions)
                .FirstOrDefaultAsync(s => s.Id == hello.Id);

            if (student == null)
            {
                return NotFound();
            }



            var lstModel = new List<SimpleReportViewModel>();


            Dictionary<string, double> categoryAmounts = new Dictionary<string, double>();
            IEnumerable<string> categories = hello.Transactions.Select(s => s.Category).Distinct().ToList();


            double totalQuantity = 0;

            foreach (var t in hello.Transactions)
            {

                totalQuantity += t.Amount;

                string cat = Enum.GetName(typeof(TransactionForm.Categories), Int32.Parse(t.Category));


                if (categoryAmounts.ContainsKey(cat))
                {
                    categoryAmounts[cat] = categoryAmounts[cat] + t.Amount;
                }
                else
                {
                    categoryAmounts.Add(cat, t.Amount);
                }

            }

            foreach (var c in categories)
            {

                string cat = Enum.GetName(typeof(TransactionForm.Categories), Int32.Parse(c));

                lstModel.Add(new SimpleReportViewModel
                {
                    DimensionOne = cat,
                    Quantity = (int)Math.Round((categoryAmounts[cat]))

                });
            }
            IEnumerable<SimpleReportViewModel> categoriesIEnum = lstModel;
            return View(categoriesIEnum);


        }

        public IActionResult SignUp()
        {
            return View();
        }

        public Boolean ValidateEmail(string email)
        {

            if(_context.Accounts.Any(a => a.Email == email))
            {
                return false;
            }
            return true;
        }

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
                    //AccountRepository.AddAccount(newAccount);

                    _context.Add(newAccount);
                    _context.SaveChanges();
                   
                    _currentUserId = newAccount.Id;

                    //_context.Accounts.Where(u => u.Email == newAccount.Email)
                    //.Select(u => u.Id)
                    //.FirstOrDefault();
                   // TempData["Account"] = newAccount.Id; 

                }

                else
                {
                    ModelState.AddModelError("Email", "Account with this email already exists.");
                    return View();
                }


                /*
                string uniqueFileName = null;
                if (model.ProfilePicture != null)
                {
                    string uploadsFolder = Path.Combine(Path., "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.ProfilePicture.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Account newAccount = new Account(model.Name, model.Email, model.Password, uniqueFileName);
                AccountRepository.AddAccount(newAccount);
                RedirectToAction("Index");*/

                return RedirectToAction("AddTransaction");

            }

            else
            {
                return View();
            }
        }

        [HttpGet]
        public ViewResult AddTransaction()
        {
            
            return View();
        }


        [HttpPost]
        public ViewResult AddTransaction(TransactionForm form)
        {
            if (ModelState.IsValid)
            {
                
                 
                //var account = _context.Accounts.Find(_currentUserId);
                //db.Books.SingleOrDefault(b => b.BookNumber == bookNumber)
                Account account = _context.Accounts.SingleOrDefault(a => a.Id == _currentUserId);

                List<Transaction> transactions = account.Transactions;

                string fileName = Path.GetFileNameWithoutExtension(form.Receipt.FileName);
                string extension = Path.GetExtension(form.Receipt.FileName);
                fileName = Guid.NewGuid().ToString() + "_" + fileName + extension;
                string filePath = Path.Combine("wwwroot/images/", fileName);
                form.Receipt.CopyTo(new FileStream(filePath, FileMode.Create));

                Transaction newTransaction = new Transaction(form.TransactionType,form.Name,form.Amount,form.Category,form.Note, fileName);
                
                account.Transactions.Add(newTransaction);

                _context.Accounts.Attach(account);
                var entry = _context.Entry(account);
                entry.Collection(a => a.Transactions).IsModified = true;

                _context.SaveChanges();

                
                return View("Dashboard");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AllReceipts()
        {
            return View();
        }

        public async Task<IActionResult> Receipt(string category)
        {
            var account = await _context
                .Accounts
                .Include(a=> a.Transactions)
                .FirstOrDefaultAsync(a=> a.Id == _currentUserId);
            var transactions = account.Transactions.Where(t => t.Category == category).ToList();

            //List<Transaction> transactions = new List<Transaction>();

            //foreach (var transaction in account.Transactions) {
            //    if (transaction.Category == "0") {
            //        transactions.Add(transaction);
            //    }
            //}


            //List<Transaction> transactions = account.Transactions.Where(t => t.Category == category).ToList();
            /*foreach (var transaction in account.Transactions)
                {
                    if (transaction.Category == category)
                    {
                        transactions.Add(transaction);
                    }
                }*/

            //return View(ReceiptRepository.GetTransactions().Where(r => r.Category == category));
            return View(transactions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
