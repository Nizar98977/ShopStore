using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShopeStore.API.Controllers;

namespace ShopeStore.API.Controllers
{
    public class ProductTypesController : BaseApiController
    {
        private readonly IGenericService<ProductType> _productTypeRepo;
        public ProductTypesController(IGenericService<ProductType> productTypeRepo)
        {
            _productTypeRepo = productTypeRepo;
        }

        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<ProductType>))]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductsTypsAsync()
        {
            var productsBrands = await _productTypeRepo.GetAllAsync();
            return Ok(productsBrands);
        }
    }
}
