using KahaTiev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KahaTiev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var authenticated = User.Identity!.IsAuthenticated;
            return View();
        }


        [Authorize(Roles = "investor")]
        public IActionResult Privacy()
        {
            return View();
        }

      
    }
}
