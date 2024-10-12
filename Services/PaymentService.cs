using KahaTiev.DTOs;
using KahaTiev.DTOs.Payment;
using KahaTiev.Models;
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
        public async Task<DataResponse> ProcessPayment(PaymentViewModel model)
        {
         



            return new DataResponse
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
