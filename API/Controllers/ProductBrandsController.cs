using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ShopeStore.API.Controllers
{
    public class ProductBrandsController : Controller
    {
        private readonly IGenericService<ProductBrand> _productbrandRepo;
        public ProductBrandsController(IGenericService<ProductBrand> productbrandRepo)
        {
            _productbrandRepo = productbrandRepo;
        }
        [HttpGet("brands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<ProductBrand>))]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductsBrandsAsync()
        {
            var productsBrands = await _productbrandRepo.GetAllAsync();
            return Ok(productsBrands);
        }
    }
}
