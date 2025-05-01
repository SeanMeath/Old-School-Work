using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Integration_Lab4.Models;

namespace Integration_Lab4.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Courses()
        {
            return View();
        }

        public IActionResult OurProgram()
        {
            return View();
        }

        public IActionResult Staff()
        {
            return View();
        }

        public IActionResult Books()
        {
            var booklist = new List<Book>();
            booklist.Add(new Book { Id = 1, Price = 1, Title = "aaa", });
            booklist.Add(new Book { Id = 2, Price = 1, Title = "bbb", });
            booklist.Add(new Book { Id = 3, Price = 1, Title = "ccc", });

            return View("Books", booklist);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
