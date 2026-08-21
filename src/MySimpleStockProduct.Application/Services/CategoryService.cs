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

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryDTOValidator _validator;
        private readonly ICustomLogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ICustomLogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = new CategoryDTOValidator();
        }

        public async Task<ResponseDTO<CategoryDTO>> CreateAsync(CategoryDTO dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Starting category creation process");

            ResponseDTO<CategoryDTO> response = new ResponseDTO<CategoryDTO>();

            if (dto is null)
            {
                _logger.LogWarning("Category creation failed: Payload is null");
                response.Fail("Category payload is null.");
                return response;
            }

            try
            {
                ValidationResult validationResult = _validator.Validate(dto);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Category creation failed due to validation errors for: {0}", dto.Name);
                    response.Fail(validationErrors: validationResult.ToDictionary());
                    return response;
                }

                Category entity = new Category(dto.Name, dto.Description);
                await _categoryRepository.AddAsync(entity, cancellationToken).ConfigureAwait(false);

                _logger.LogInfo("Category '{0}' created successfully with ID: {1}", entity.Name, entity.Id);

                response.Ok(CategoryMapper.ToDto(entity));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating category '{0}'", dto.Name);
                response.FromException(ex);
                return response;
            }
        }

        public async Task<ResponseDTO<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Starting category deletion for ID: {0}", id);

            ResponseDTO<bool> response = new ResponseDTO<bool>();

            if (id == Guid.Empty)
            {
                _logger.LogWarning("Category deletion failed: Invalid ID provided");
                response.Fail("Invalid id.");
                return response;
            }

            Category? exists = await _categoryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (exists is null)
            {
                _logger.LogWarning("Category deletion failed: Category with ID {0} not found", id);
                response.Fail("Category not found.");
                return response;
            }

            try
            {
                await _categoryRepository.DeleteAsync(exists, cancellationToken).ConfigureAwait(false);

                _logger.LogInfo("Category with ID {0} deleted successfully", id);

                response.Ok(true);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting category with ID: {0}", id);
                response.FromException(ex);
                return response;
            }
        }

        public async Task<ResponseDTO<IEnumerable<CategoryDTO>>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Fetching categories list. Page: {0}, PageSize: {1}", page, pageSize);

            ResponseDTO<IEnumerable<CategoryDTO>> response = new ResponseDTO<IEnumerable<CategoryDTO>>();

            IReadOnlyList<Category> entities = await _categoryRepository.GetAsync(page, pageSize, cancellationToken).ConfigureAwait(false);
            List<CategoryDTO> dtos = entities.Select(CategoryMapper.ToDto).ToList();

            _logger.LogInfo("Retrieved {0} categories for page {1}", dtos.Count, page);

            response.Ok(dtos);
            return response;
        }

        public async Task<ResponseDTO<CategoryDTO>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Fetching category by ID: {0}", id);

            ResponseDTO<CategoryDTO> response = new ResponseDTO<CategoryDTO>();

            if (id == Guid.Empty)
            {
                _logger.LogWarning("GetById failed: Invalid ID provided");
                response.Fail("Invalid id.");
                return response;
            }

            Category? entity = await _categoryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (entity is null)
            {
                _logger.LogWarning("Category with ID {0} was not found", id);
                response.Fail("Category not found.");
                return response;
            }

            _logger.LogInfo("Successfully retrieved category with ID: {0}", id);

            response.Ok(CategoryMapper.ToDto(entity));
            return response;
        }

        public async Task<ResponseDTO<CategoryDTO>> UpdateAsync(Guid id, CategoryDTO dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Starting category update for ID: {0}", id);

            ResponseDTO<CategoryDTO> response = new ResponseDTO<CategoryDTO>();

            if (id == Guid.Empty || dto is null)
            {
                _logger.LogWarning("Category update failed: Invalid ID or payload is null");
                response.Fail("Invalid input.");
                return response;
            }

            Category? existing = await _categoryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (existing is null)
            {
                _logger.LogWarning("Category update failed: Category with ID {0} not found", id);
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
                    _logger.LogWarning("Category update failed due to validation errors for ID: {0}", id);
                    response.Fail(validationErrors: validationResult.ToDictionary());
                    return response;
                }

                await _categoryRepository.UpdateAsync(existing, cancellationToken).ConfigureAwait(false);

                _logger.LogInfo("Category with ID {0} updated successfully", id);

                response.Ok(CategoryMapper.ToDto(existing));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating category with ID: {0}", id);
                response.FromException(ex);
                return response;
            }
        }
    }
}
