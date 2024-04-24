using Core.Entites;

namespace Core.Specification
{
    public class ProductsWithTypsAndBrandsSpecfication : BaseSpecification<Product>
    {
        public ProductsWithTypsAndBrandsSpecfication()
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
        public ProductsWithTypsAndBrandsSpecfication(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}
