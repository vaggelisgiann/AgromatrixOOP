using Agromatrix.Data;
using Agromatrix.Models;
using Microsoft.EntityFrameworkCore;

namespace Agromatrix.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddProductAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(Product product)
{
    var existingProduct = await _db.Products.FindAsync(product.Id);
    if (existingProduct == null) return;

    existingProduct.Name = product.Name;
    existingProduct.Description = product.Description;
    existingProduct.Category = product.Category;
    existingProduct.Price = product.Price;
    existingProduct.ImageUrl = product.ImageUrl;

    await _db.SaveChangesAsync();
}


    }
}