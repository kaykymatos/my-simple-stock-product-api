using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Domain.Entities;

namespace MySimpleStockProduct.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductDTO ToDto(Product Product)
        {
            return new ProductDTO
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description
            };
        }

        public static Product ToEntity(ProductDTO dto)
        {
            return new Product(dto.Name, dto.Description, dto.Price, dto.CategoryId);
        }
        public static IList<ProductDTO> ToDto(IList<Product> categories)
        {
            IList<ProductDTO> dtos = new List<ProductDTO>();
            foreach (Product Product in categories)
            {
                dtos.Add(ToDto(Product));
            }
            return dtos;
        }
    }
}
