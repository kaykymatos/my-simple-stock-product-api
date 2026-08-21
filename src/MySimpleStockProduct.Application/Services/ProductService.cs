using FluentValidation.Results;
using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Application.Interfaces;
using MySimpleStockProduct.Application.Mappers;
using MySimpleStockProduct.Application.Validators;
using MySimpleStockProduct.Domain.Entities;
using MySimpleStockProduct.Domain.Interfaces;

namespace MySimpleStockProduct.Application.Services
{
    using MySimpleStockProduct.Logging;

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ProductDTOValidator _validator;
        private readonly ICustomLogger<ProductService> _logger;

        public ProductService(IProductRepository ProductRepository, ICategoryRepository CategoryRepository, ICustomLogger<ProductService> logger)
        {
            _productRepository = ProductRepository ?? throw new ArgumentNullException(nameof(ProductRepository));
            _categoryRepository = CategoryRepository ?? throw new ArgumentNullException(nameof(CategoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = new ProductDTOValidator();
        }

        public async Task<ResponseDTO<ProductDTO>> CreateAsync(ProductDTO dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Starting product creation process");

            ResponseDTO<ProductDTO> response = new ResponseDTO<ProductDTO>();

            if (dto is null)
            {
                _logger.LogWarning("Product creation failed: Payload is null");
                response.Fail("Product payload is null.");
                return response;
            }

            Category? category = await _categoryRepository.GetByIdAsync(dto.CategoryId, cancellationToken).ConfigureAwait(false);
            if (category is null)
            {
                _logger.LogWarning("Product creation failed: Category with ID {0} not found", dto.CategoryId);
                response.Fail("Category not found.");
                return response;
            }
            Product entity = ProductMapper.ToEntity(dto);
            try
            {
                ValidationResult validationResult = _validator.Validate(dto);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Product creation failed due to validation errors for: {0}", dto.Name);
                    response.Fail(validationErrors: validationResult.ToDictionary());
                    return response;
                }
                await _productRepository.AddAsync(entity, cancellationToken).ConfigureAwait(false);

                _logger.LogInfo("Product '{0}' created successfully with ID: {1}", entity.Name, entity.Id);

                response.Ok(ProductMapper.ToDto(entity));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating product '{0}'", dto?.Name);
                response.FromException(ex);
                return response;
            }

        }

        public async Task<ResponseDTO<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Starting product deletion for ID: {0}", id);

            ResponseDTO<bool> response = new ResponseDTO<bool>();

            if (id == Guid.Empty)
            {
                _logger.LogWarning("Product deletion failed: Invalid ID provided");
                response.Fail("Invalid id.");
                return response;
            }

            Product? exists = await _productRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (exists is null)
            {
                _logger.LogWarning("Product deletion failed: Product with ID {0} not found", id);
                response.Fail("Product not found.");
                return response;
            }

            try
            {
                await _productRepository.DeleteAsync(exists, cancellationToken).ConfigureAwait(false);

                _logger.LogInfo("Product with ID {0} deleted successfully", id);

                response.Ok(true);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID: {0}", id);
                response.FromException(ex);
                return response;
            }
        }

        public async Task<ResponseDTO<IEnumerable<ProductDTO>>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Fetching products list. Page: {0}, PageSize: {1}", page, pageSize);

            ResponseDTO<IEnumerable<ProductDTO>> response = new ResponseDTO<IEnumerable<ProductDTO>>();

            IReadOnlyList<Product> entities = await _productRepository.GetAsync(page, pageSize, cancellationToken).ConfigureAwait(false);
            List<ProductDTO> dtos = entities.Select(ProductMapper.ToDto).ToList();

            _logger.LogInfo("Retrieved {0} products for page {1}", dtos.Count, page);

            response.Ok(dtos);
            return response;
        }

        public async Task<ResponseDTO<ProductDTO>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Fetching product by ID: {0}", id);

            ResponseDTO<ProductDTO> response = new ResponseDTO<ProductDTO>();

            if (id == Guid.Empty)
            {
                _logger.LogWarning("GetById failed: Invalid ID provided");
                response.Fail("Invalid id.");
                return response;
            }

            Product? entity = await _productRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (entity is null)
            {
                _logger.LogWarning("Product with ID {0} was not found", id);
                response.Fail("Product not found.");
                return response;
            }

            _logger.LogInfo("Successfully retrieved product with ID: {0}", id);

            response.Ok(ProductMapper.ToDto(entity));
            return response;
        }

        public async Task<ResponseDTO<ProductDTO>> UpdateAsync(Guid id, ProductDTO dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Starting product update for ID: {0}", id);

            ResponseDTO<ProductDTO> response = new ResponseDTO<ProductDTO>();

            if (id == Guid.Empty || dto is null)
            {
                _logger.LogWarning("Product update failed: Invalid ID or payload is null");
                response.Fail("Invalid input.");
                return response;
            }

            Product? existing = await _productRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (existing is null)
            {
                _logger.LogWarning("Product update failed: Product with ID {0} not found", id);
                response.Fail("Product not found.");
                return response;
            }

            Category? category = await _categoryRepository.GetByIdAsync(dto.CategoryId, cancellationToken).ConfigureAwait(false);
            if (category is null)
            {
                _logger.LogWarning("Product update failed: Category with ID {0} not found", dto.CategoryId);
                response.Fail("Category not found.");
                return response;
            }

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.UpdatedAt = DateTime.UtcNow;
            try
            {
                ValidationResult validationResult = _validator.Validate(dto);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Product update failed due to validation errors for: {0}", dto.Name);
                    response.Fail(validationErrors: validationResult.ToDictionary());
                    return response;
                }
                await _productRepository.UpdateAsync(existing, cancellationToken).ConfigureAwait(false);

                _logger.LogInfo("Product '{0}' updated successfully with ID: {1}", existing.Name, existing.Id);

                response.Ok(ProductMapper.ToDto(existing));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product with ID: {0}", id);
                response.FromException(ex);
                return response;
            }
        }
    }
}
