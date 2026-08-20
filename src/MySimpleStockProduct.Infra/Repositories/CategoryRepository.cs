using Microsoft.EntityFrameworkCore;
using MySimpleStockProduct.Domain.Entities;
using MySimpleStockProduct.Domain.Interfaces;
using MySimpleStockProduct.Infra.Context;

namespace MySimpleStockProduct.Infra.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
