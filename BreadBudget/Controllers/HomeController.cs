﻿using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BreadBudget.Models;

namespace BreadBudget.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult SignUp()
        {

            return View();
        }
        public IActionResult Dashboard()
        {

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public ViewResult AddTransaction()
        {
            return View();
        }


        [HttpPost]
        public ViewResult AddTransaction(TransactionForm transactionform)
        {
            if (ModelState.IsValid)
            {
                TransactionRepository.AddForm(transactionform);
                return View("Conformation", transactionform);
            }
            else
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

     
    }
}
