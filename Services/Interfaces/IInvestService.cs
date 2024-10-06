using KahaTiev.DTOs;

namespace KahaTiev.Services.Interfaces
{
    public interface IInvestService
    {
        Task<List<ProductDTO>> Products();
    }
}
