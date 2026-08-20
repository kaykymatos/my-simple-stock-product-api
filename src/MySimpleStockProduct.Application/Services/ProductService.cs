using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Application.Interfaces;
using MySimpleStockProduct.Application.Mappers;
using MySimpleStockProduct.Domain.Entities;
using MySimpleStockProduct.Domain.Interfaces;

namespace MySimpleStockProduct.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _ProductRepository;

        public ProductService(IProductRepository ProductRepository)
        {
            _ProductRepository = ProductRepository ?? throw new ArgumentNullException(nameof(ProductRepository));
        }

        public async Task<ResponseDTO<ProductDTO>> CreateAsync(ProductDTO dto, CancellationToken cancellationToken = default)
        {
            ResponseDTO<ProductDTO> response = new ResponseDTO<ProductDTO>();

            if (dto is null)
            {
                response.Fail([], "Product payload is null.");
                return response;
            }
            Product entity = ProductMapper.ToEntity(dto);
            await _ProductRepository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            response.Ok(ProductMapper.ToDto(entity));
            return response;
        }

        public async Task<ResponseDTO<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            ResponseDTO<bool> response = new ResponseDTO<bool>();

            if (id == Guid.Empty)
            {
                response.Fail([], "Invalid id.");
                return response;
            }

            Product? exists = await _ProductRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (exists is null)
            {
                response.Fail([], "Product not found.");
                return response;
            }

            try
            {
                await _ProductRepository.DeleteAsync(exists, cancellationToken).ConfigureAwait(false);
                return response;
            }
            catch (Exception ex)
            {
                return response.FromException(ex);
            }
        }

        public async Task<ResponseDTO<IEnumerable<ProductDTO>>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            ResponseDTO<IEnumerable<ProductDTO>> response = new ResponseDTO<IEnumerable<ProductDTO>>();

            IReadOnlyList<Product> entities = await _ProductRepository.GetAsync(page, pageSize, cancellationToken).ConfigureAwait(false);
            List<ProductDTO> dtos = entities.Select(ProductMapper.ToDto).ToList();

            response.Ok(dtos);
            return response;
        }

        public async Task<ResponseDTO<ProductDTO>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            ResponseDTO<ProductDTO> response = new ResponseDTO<ProductDTO>();

            if (id == Guid.Empty)
            {
                response.Fail([], "Invalid id.");
                return response;
            }

            Product? entity = await _ProductRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (entity is null)
            {
                response.Fail([], "Product not found.");
                return response;
            }

            response.Ok(ProductMapper.ToDto(entity));
            return response;
        }

        public async Task<ResponseDTO<ProductDTO>> UpdateAsync(Guid id, ProductDTO dto, CancellationToken cancellationToken = default)
        {
            ResponseDTO<ProductDTO> response = new ResponseDTO<ProductDTO>();

            if (id == Guid.Empty || dto is null)
            {
                response.Fail([], "Invalid input.");
                return response;
            }

            Product? existing = await _ProductRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (existing is null)
            {
                response.Fail([], "Product not found.");
                return response;
            }

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.UpdatedAt = DateTime.UtcNow;

            await _ProductRepository.UpdateAsync(existing, cancellationToken).ConfigureAwait(false);
            response.Ok(ProductMapper.ToDto(existing));
            return response;
        }
    }
}
