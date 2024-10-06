using Microsoft.AspNetCore.Mvc;

namespace KahaTiev.Controllers
{
    public class InvestController : Controller
    {
       public async Task<IActionResult> Product()
        {
            return View();
        }
    }
}
