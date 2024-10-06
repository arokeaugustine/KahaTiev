using KahaTiev.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KahaTiev.Controllers
{
    public class InvestController : Controller
    {
        private readonly IInvestService _investService;
        public InvestController(IInvestService investService)
        {
            _investService = investService;
        }
        public async Task<IActionResult> Product()
        {
            var products = await _investService.Products();
            return View();
        }
    }
}
