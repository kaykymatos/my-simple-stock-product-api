using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Domain.Entities;

namespace MySimpleStockProduct.Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDTO ToDto(Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public static Category ToEntity(CategoryDTO dto)
        {
            return new Category(dto.Name, dto.Description);
        }
        public static IList<CategoryDTO> ToDto(IList<Category> categories)
        {
            IList<CategoryDTO> dtos = new List<CategoryDTO>();
            foreach (Category category in categories)
            {
                dtos.Add(ToDto(category));
            }
            return dtos;
        }
    }
}
