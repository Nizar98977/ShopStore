using API.DTOs;
using AutoMapper;
using Core.Entites;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Errors;

namespace Skinet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseApiController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            ILogger<ProductsController> logger,
            IGenericRepository<Product> productRepo,
            IMapper mapper
            )
        {
            _logger = logger;
            _productRepo = productRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetProducts()
        {
            _logger.LogInformation("Retrieving products...");
            var products = await _productRepo.GetAllAsync(p => p.ProductType, products => products.ProductBrand);
            _logger.LogInformation($"Retrieved {products.Count()} products");

            return Ok(_mapper.Map<IEnumerable<Product>, IReadOnlyList<ProductDTO>>(products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            _logger.LogInformation($"Retrieving product with ID {id}");
            var product = await _productRepo.GetByIdAsync(id, p => p.ProductType, p => p.ProductBrand);

            if (product != null)
            {
                _logger.LogInformation($"Product with ID {id} retrieved successfully.");
                return _mapper.Map<Product, ProductDTO>(product);
            }
            else
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return NotFound(new ApiResponse(404));
            }
        }


        [HttpPut("UpdateProduct/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Product>> UpdateProduct(int id, ProductDTO productDto)
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
        public async Task<ActionResult<Product>> AddProduct(ProductDTO productDto)
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