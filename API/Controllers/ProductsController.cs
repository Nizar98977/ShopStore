using API.DTOs;
using AutoMapper;
using Core.Entites;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productbrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            ILogger<ProductsController> logger,
            IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> productbrandRepo,
            IGenericRepository<ProductType> productTypeRepo,
            IMapper mapper
            )
        {
            _logger = logger;
            _productRepo = productRepo;
            _productbrandRepo = productbrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            _logger.LogInformation("Retrieving products...");
            var spec = new ProductsWithTypsAndBrandsSpecfication();
            var products = await _productRepo.ListAsync(spec);
            _logger.LogInformation($"Retrieved {products.Count} products");

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductsBrandsAsync()
        {
            var productsBrands = await _productbrandRepo.ListAllAsync();
            return Ok(productsBrands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductsTypsAsync()
        {
            var productsBrands = await _productTypeRepo.ListAllAsync();
            return Ok(productsBrands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            _logger.LogInformation($"Retrieving product with ID {id}");

            var spec = new ProductsWithTypsAndBrandsSpecfication(id);
            var product = await _productRepo.GetEntityWithSpec(spec);
            if (product != null)
            {
                _logger.LogInformation($"Product with ID {id} retrieved successfully.");
                return _mapper.Map<Product, ProductToReturnDto>(product);
            }
            else
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return NotFound();
            }
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, ProductToReturnDto productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the product data.");
            }

            var existingProduct = await _productRepo.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }
            //use automapper to udpate the product here 
            await _productRepo.UpdateAsync(existingProduct);

            _logger.LogInformation($"Product with ID {id} is updated successfully");

            return NoContent();
        }

        [HttpPost("AddProduct")]
        public async Task<ActionResult<Product>> AddProduct(ProductToReturnDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add automapper here to map from the Dto to the Product
            await _productRepo.InsertAsync(new Product { });

            _logger.LogInformation($"Product with ID {productDto.Id} is added successfully");

            return CreatedAtAction(nameof(GetProduct), new { id = productDto.Id }, productDto);
        }
    }
}