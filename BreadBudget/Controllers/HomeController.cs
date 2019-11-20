﻿using System;
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
using NewsAPI.Constants;
using NewsAPI.Models;

namespace BreadBudget.Controllers
{
    public class HomeController : Controller

    {
        private UserDb _context;

        // current user account 
        private int _currentUserId { get; set; }

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
                        return View("Dashboard", queryAccount);
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

        public IActionResult DisplayTest()
        {
            return View(_context.Accounts.ToList());
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
                    _currentUserId = _context.Accounts.Where(u => u.Email == newAccount.Email)
                        .Select(u => u.Id)
                        .FirstOrDefault();

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
                TransactionRepository.AddForm(form);
               
                return View("Conformation", form);
            }
            else
            {
                return View();
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AllReceipts()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public ArticlesResult News()
        {

            var newsApiClient = new NewsApiClient("465ab82acc5b43999381d54823215a61");
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = "Apple",
                SortBy = SortBys.Popularity,
                Language = Languages.EN
            });
            return articlesResponse;
        }

    }
}
