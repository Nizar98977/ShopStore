using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopeStore.API.Errors;


namespace Infrastructure.ExtensionServices
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Setup
            services.AddDbContext<StoreContext>(opt =>
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Repositories with Dependency Injection
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // AutoMapper Configuration (alternative approach)
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Model State Validation Error Handling
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(e => e.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = errors });
                };
            });
        }
    }
}
