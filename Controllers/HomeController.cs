using MestreDigital.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MestreDigital.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()

        {
            return View();
        }
       
    }
}