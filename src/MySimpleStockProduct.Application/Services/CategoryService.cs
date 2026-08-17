using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Application.Interfaces;

namespace MySimpleStockProduct.Application.Services
{
    public class CategoryService : ICategoryService
    {
        public Task<ResponseDTO<CategoryDTO>> CreateAsync(CategoryDTO dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<ProductDTO>> CreateAsync(ProductDTO dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<IEnumerable<CategoryDTO>>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<CategoryDTO>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<CategoryDTO>> UpdateAsync(Guid id, CategoryDTO dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<ProductDTO>> UpdateAsync(Guid id, ProductDTO dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        Task<ResponseDTO<IEnumerable<ProductDTO>>> IBaseService<ProductDTO>.GetAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<ResponseDTO<ProductDTO>> IBaseService<ProductDTO>.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
