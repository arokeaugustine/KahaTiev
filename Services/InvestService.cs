using KahaTiev.DTOs;
using KahaTiev.Models;
using KahaTiev.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KahaTiev.Services
{
    public class InvestService : IInvestService
    {
        private readonly KahaTievContext _context;
        public InvestService(KahaTievContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDTO>> Products()
        {
            var products = await _context.Products
                .Where(x => x.IsActive && !x.IsDeleted)
                .Select(x => new ProductDTO
                {
                    Name = x.Name,
                    Id = x.Id,
                    Guid = x.Guid,
                    Description = x.Description
                })
                .ToListAsync();
            return products;
        }


    }
}
