using KahaTiev.DTOs;
using KahaTiev.DTOs.Payment;
using KahaTiev.Models;

namespace KahaTiev.Services.Interfaces
{
    public interface IInvestService
    {
        Task<List<ProductDTO>> Products();
        Task<Product?> Packages(Guid guid);
        Task<PaymentViewModel> Package(Guid guid);
    }
}
