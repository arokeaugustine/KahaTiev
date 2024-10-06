using KahaTiev.DTOs;
using KahaTiev.Models;

namespace KahaTiev.Services.Interfaces
{
    public interface IInvestService
    {
        Task<List<ProductDTO>> Products();
        Task<List<Product>> Packages(Guid guid);
    }
}
