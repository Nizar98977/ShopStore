using API.DTOs;
using AutoMapper;
using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShopeStore.API.Errors;
using System.Linq.Expressions;

namespace ShopeStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseApiController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(
            ILogger<ProductsController> logger,
            IProductService productService,
            IMapper mapper
            )
        {
            _logger = logger;
            _productService = productService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetProducts(string sortBy = null)
        {
            try
            {
                _logger.LogInformation("Retrieving products...");

                if (sortBy == null)
                {
                    sortBy = "Name";
                }

                string[] sortByParts = sortBy.Split('-');
                string propertyName = sortByParts[0];
                string direction = sortByParts.Length > 1 ? sortByParts[1] : "asc";
                bool isDescending = direction.Equals("desc", StringComparison.OrdinalIgnoreCase) ? true : false;

                // Dynamic sorting logic
                Expression<Func<Product, object>> orderByExpression = GetOrderByExpression(propertyName);

                var products = await _productService.GetAllAsync(orderByExpression, isDescending, p => p.ProductType, p => p.ProductBrand);

                _logger.LogInformation($"Retrieved {products.Count()} products");

                if (!products.Any())
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<Product>, IReadOnlyList<ProductDTO>>(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products: {Message}", ex.Message);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            _logger.LogInformation($"Retrieving product with ID {id}");
            var product = await _productService.GetByIdAsync(id, p => p.ProductType, p => p.ProductBrand);

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

        [HttpPost("AddProduct")]
        public async Task<ActionResult<Product>> AddProduct(ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add automapper here to map from the Dto to the Product
            await _productService.InsertAsync(new Product { });

            _logger.LogInformation($"Product with ID {productDto.Id} is added successfully");

            return CreatedAtAction(nameof(GetProduct), new { id = productDto.Id }, productDto);
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

            var existingProduct = await _productService.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }
            //use automapper to udpate the product here 
            await _productService.UpdateAsync(existingProduct);

            _logger.LogInformation($"Product with ID {id} is updated successfully");

            return NoContent();
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var actorDetails = await _productService.GetByIdAsync(id);
            if (actorDetails == null)
            {
                _logger.LogError($"Product with ID {id} not found");
                return NotFound("Product you are trying to delete is not found");
            }
            await _productService.DeleteAsync(id);

            return Ok("Product deleted successfully");
        }

        // Method to dynamically generate the OrderBy expression
        private Expression<Func<Product, object>> GetOrderByExpression(string sortBy)
        {
            var parameter = Expression.Parameter(typeof(Product), "p");
            var property = Expression.Property(parameter, sortBy);
            var convertedProperty = Expression.Convert(property, typeof(object)); // Explicitly convert property to object
            var lambda = Expression.Lambda<Func<Product, object>>(convertedProperty, parameter);
            return lambda;
        }
    }
}