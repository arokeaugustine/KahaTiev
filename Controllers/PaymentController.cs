using KahaTiev.DTOs.Payment;
using Microsoft.AspNetCore.Mvc;
using PayStack.Net;

namespace KahaTiev.Controllers
{
    public class PaymentController : Controller
    {
      

        public async Task<IActionResult> Index(PaymentViewModel model)
        {



            return View();
        }

        public IActionResult Verify()
        {
            return View();
        }
    }
}
