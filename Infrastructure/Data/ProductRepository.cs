using Core.Entites;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBransdsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                         .Include(p => p.ProductType)
                         .Include(p => p.ProductBrand)
                         .FirstOrDefaultAsync(x => x.Id == id);

            return product ?? throw new ArgumentException($"Product with id {id} not found."); // check if null throw error
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypsAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}
