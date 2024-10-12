using KahaTiev.DTOs;
using KahaTiev.DTOs.Payment;

namespace KahaTiev.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<DataResponse> ProcessPayment(PaymentViewModel model);
    }
}
