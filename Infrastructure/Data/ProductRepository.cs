using Core.Entites;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;

namespace Infrastructure.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductService
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context, ILogger<ProductRepository> logger) : base(context, logger)
        {
            _context = context;
        }
    }
}