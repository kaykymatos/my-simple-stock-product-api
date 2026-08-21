using MySimpleStockProduct.Domain.Entities;
using MySimpleStockProduct.Domain.Interfaces;
using MySimpleStockProduct.Infra.Context;
using MySimpleStockProduct.Logging;

namespace MySimpleStockProduct.Infra.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context, ICustomLogger<BaseRepository<Product>> logger) : base(context, logger)
        {
        }
    }
}
