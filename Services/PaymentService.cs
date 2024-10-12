using KahaTiev.DTOs;
using KahaTiev.DTOs.Payment;
using KahaTiev.Models;
using PayStack.Net;

namespace KahaTiev.Services
{

    public class PaymentService
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
        public async Task<Response> ProcessPayment(PaymentViewModel model)
        {
            TransactionInitializeRequest request = new TransactionInitializeRequest
            {
                AmountInKobo = model.PaystackDTO.Amount * 100,
                Email = model.PaystackDTO.Email,
                Reference = GenerateTransactionRef(),
                Currency = "NGN",
                CallbackUrl = "http://localhost:22925/Payment/Verify"  
            };

            TransactionInitializeResponse response = payStackApi.Transactions.Initialize(request);
            if (response.Status)
            {
                var transaction = new Transaction
                {
                    Amount = model.PaystackDTO.Amount,
                    Email = model.PaystackDTO.Email,
                    Name = model.PaystackDTO.Name,
                    TransactionReference = request.Reference
                }; 

                _kahaTievContext.Transactions.Add(transaction); 
                var save = await _kahaTievContext.SaveChangesAsync();
                if (save > 0)
                {
                    return new Response
                    {
                        status = true,
                        message = "Transaction Intialized successfully"
                    };
                }

            }



            return new Response
            {
                status = false,
                message = "An error occured!"
            };


        }



        private static string GenerateTransactionRef()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(10000000, 9999999).ToString();
        }
    }
}
