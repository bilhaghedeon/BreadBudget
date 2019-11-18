using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BreadBudget.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BreadBudget.Controllers
{
    public class HomeController : Controller

    {


        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(
            IHostingEnvironment hostingEnvironmnet)
        {
           
            this.hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            
           
            return View();
        }

        public IActionResult DisplayTest(Account account)
        {


            return View();
        }

        public IActionResult SignUp()
        {


            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUp model)
        {
            if (ModelState.IsValid)
            {

                string fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
                string extension = Path.GetExtension(model.ProfilePicture.FileName);
                fileName = Guid.NewGuid().ToString() + "_" + fileName + extension;
                string filePath = Path.Combine("wwwroot/images/", fileName);
                model.ProfilePicture.CopyTo(new FileStream(filePath, FileMode.Create));
                Account newAccount = new Account(model.Name, model.Email, model.Password, fileName);
                AccountRepository.AddAccount(newAccount);

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

                return View("DisplayTest", newAccount);

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
