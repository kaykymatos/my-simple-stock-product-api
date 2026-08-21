using MySimpleStockProduct.Domain.Entities;
using MySimpleStockProduct.Domain.Interfaces;
using MySimpleStockProduct.Infra.Context;
using MySimpleStockProduct.Logging;

namespace MySimpleStockProduct.Infra.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context, ICustomLogger<BaseRepository<Category>> logger) : base(context, logger)
        {
        }
    }
}
