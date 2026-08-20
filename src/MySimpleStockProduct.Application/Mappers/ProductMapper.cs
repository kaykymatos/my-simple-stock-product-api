using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Domain.Entities;

namespace MySimpleStockProduct.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductDTO ToDto(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Price = product.Price
            };
        }

        public static Product ToEntity(ProductDTO dto)
        {
            return new Product(dto.Name, dto.Description, dto.Price, dto.CategoryId);
        }
    }
}
