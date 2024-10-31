using KahaTiev.Data.DTOs.Payment;
using PayStack.Net;

namespace KahaTiev.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<TransactionInitializeResponse> ProcessPayment(PaymentViewModel model);
    }
}
