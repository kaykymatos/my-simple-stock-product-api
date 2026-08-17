using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Application.Interfaces;

namespace MySimpleStockProduct.Application.Services
{
    public class ProductService : IProductService
    {
        public Task<ResponseDTO<ProductDTO>> CreateAsync(ProductDTO dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<IEnumerable<ProductDTO>>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<ProductDTO>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<ProductDTO>> UpdateAsync(Guid id, ProductDTO dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
