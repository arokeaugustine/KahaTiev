using KahaTiev.DTOs.Payment;
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
            var initialization = await _paymentService.ProcessPayment(model);
            TransactionInitializeRequest request = new TransactionInitializeRequest
            {
                AmountInKobo = (int)(model.PackageAmount * 100),
                Email = model.PayerEmail,
                Reference = GenerateTransactionRef(),
                Currency = "NGN",
                CallbackUrl = "https://localhost:7098/Payment/Verify"
            };

            TransactionInitializeResponse response = payStackApi.Transactions.Initialize(request);
            if (response.Status)
            {
                var transaction = new Transaction
                {
                    Amount = model.PackageAmount,
                    Email = model.PayerEmail,
                    Name = model.PayerEmail,
                    TransactionReference = request.Reference
                };

                _kahaTievContext.Transactions.Add(transaction);
                _ = await _kahaTievContext.SaveChangesAsync();
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
