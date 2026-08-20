using Microsoft.EntityFrameworkCore;
using MySimpleStockProduct.Domain.Entities;
using MySimpleStockProduct.Domain.Interfaces;
using MySimpleStockProduct.Infra.Context;

namespace MySimpleStockProduct.Infra.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
