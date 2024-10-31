using KahaTiev.Data.DTOs.Payment;
using KahaTiev.Data.Enums;
using KahaTiev.Data.Models;
using KahaTiev.Services.Interfaces;
using PayStack.Net;

namespace KahaTiev.Services
{

    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly KahaTievContext _kahaTievContext;
        private readonly string token;

        private PayStackApi payStackApi { get; set; }
 
        public PaymentService(IConfiguration configuration, KahaTievContext kahaTievContext)
        {
            _configuration = configuration; 
            token = _configuration["PayStack:Secret-Key"];
            payStackApi = new PayStackApi(token);
            _kahaTievContext = kahaTievContext;
        }
        public async Task<TransactionInitializeResponse> ProcessPayment(PaymentViewModel model)
        {

            TransactionInitializeRequest request = new TransactionInitializeRequest
            {
                AmountInKobo = (int)(model.PackageAmount * 100),
                Email = model.PayerEmail,
                Reference = GenerateTransactionRef(),
                Currency = "NGN",
                CallbackUrl = _configuration["PayStack:callBackUrl"]
            };

            TransactionInitializeResponse response = payStackApi.Transactions.Initialize(request);

            if (response.Status)
            {
                var transaction = new Transaction
                {
                    Amount = model.PackageAmount,
                    Email = model.PayerEmail,
                    Name = model.PayerEmail,
                    TransactionReference = request.Reference,
                    TransactionType = nameof(TransactionType.Credit)
                };

                var addTrans =await _kahaTievContext.Transactions.AddAsync(transaction);

                var save = await _kahaTievContext.SaveChangesAsync();
                return response;
            }

            return response;

        }



        private static string GenerateTransactionRef()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(1000, 9999999).ToString();
        }
    }
}
