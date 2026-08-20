using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySimpleStockProduct.Application.Interfaces;
using MySimpleStockProduct.Application.Services;
using MySimpleStockProduct.Domain.Interfaces;
using MySimpleStockProduct.Infra.Context;
using MySimpleStockProduct.Infra.Repositories;

namespace MySimpleStockProduct.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly(
                        typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }

    }
}
