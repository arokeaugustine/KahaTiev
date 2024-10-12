﻿using KahaTiev.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KahaTiev.Controllers
{
    [Authorize]
    public class InvestController : Controller
    {
        private readonly IInvestService _investService;
        public InvestController(IInvestService investService)
        {
            _investService = investService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _investService.Products();
            return View(products);
        }

        public async Task<IActionResult> Packages(Guid id)
        {
            var packages = await _investService.Packages(id);
            if (packages == null)
            {
               /* Add a temp data variable that will send an alert to the index page when packages are null*/
                return RedirectToAction("Index");
            }
            return View(packages);
        }

    }
}
