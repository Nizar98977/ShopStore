using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepository;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductsBrandsAsync()
        {
            var productsBrands = await _productRepository.GetProductBransdsAsync();
            return Ok(productsBrands);
        }
        [HttpGet("typs")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductsTypsAsync()
        {
            var productsBrands = await _productRepository.GetProductTypsAsync();
            return Ok(productsBrands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}