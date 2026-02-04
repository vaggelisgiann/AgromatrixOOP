using Agromatrix.Models;

namespace Agromatrix.Services
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task UpdateAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}