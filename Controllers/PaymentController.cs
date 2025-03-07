﻿using KahaTiev.Data.DTOs.Payment;
using KahaTiev.Models;
using KahaTiev.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PayStack.Net;

namespace KahaTiev.Controllers
{
    public class PaymentController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly KahaTievContext _kahaTievContext;
        private readonly string token;

        private PayStackApi payStackApi { get; set; }
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService, IConfiguration configuration, KahaTievContext kahaTievContext)
        {
            _paymentService = paymentService;

            _configuration = configuration;
            token = _configuration["PayStack:Secret-Key"];
            payStackApi = new PayStackApi(token);
            _kahaTievContext = kahaTievContext;
        }

        public async Task<IActionResult> Index(PaymentViewModel model)
        {
            var response = await _paymentService.ProcessPayment(model);
            
            if (response.Status)
            {             
               return Redirect(response.Data.AuthorizationUrl);
            }
            TempData["error"] = response.Message; 
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Verify(string trxref, string reference)
        {
            TransactionVerifyResponse response = payStackApi.Transactions.Verify(trxref);
            if (response.Data.Status == "success")
            {
                var transaction = await _kahaTievContext.Transactions.FirstOrDefaultAsync(x => x.TransactionReference == trxref);
                if (transaction != null)
                {
                    transaction.IsSuccessful = true;
                    await _kahaTievContext.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["error"] = response.Data.GatewayResponse;
            return View();
        }


        private static string GenerateTransactionRef()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(1000, 9999999).ToString();
        }
    }
}
