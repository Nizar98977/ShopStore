using Core.Entites;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandData = await File.ReadAllTextAsync("../Infrastructure/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                context.ProductBrands.AddRange(brands);
            }
            if (!context.ProductTypes.Any())
            {
                var typesData = await File.ReadAllTextAsync("../Infrastructure/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
            }
            if (!context.Products.Any())
            {
                var productData = await File.ReadAllTextAsync("../Infrastructure/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                context.Products.AddRange(products);
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}