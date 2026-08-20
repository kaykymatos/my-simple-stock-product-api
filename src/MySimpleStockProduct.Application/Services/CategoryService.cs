using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Application.Interfaces;
using MySimpleStockProduct.Application.Mappers;
using MySimpleStockProduct.Domain.Entities;
using MySimpleStockProduct.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MySimpleStockProduct.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<ResponseDTO<CategoryDTO>> CreateAsync(CategoryDTO dto, CancellationToken cancellationToken = default)
        {
            var response = new ResponseDTO<CategoryDTO>();

            if (dto is null)
            {
                response.Fail([], "Category payload is null.");
                return response;
            }

            var entity = new Category(dto.Name, dto.Description);
            await _categoryRepository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            response.Ok(CategoryMapper.ToDto(entity));
            return response;
        }

        public async Task<ResponseDTO<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var response = new ResponseDTO<bool>();

            if (id == Guid.Empty)
            {
                response.Fail([], "Invalid id.");
                return response;
            }

            var exists = await _categoryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (exists is null)
            {
                response.Fail([], "Category not found.");
                return response;
            }

            try
            {
                await _categoryRepository.DeleteAsync(exists, cancellationToken).ConfigureAwait(false);
                return response;
            }
            catch (Exception ex)
            {
                return response.FromException(ex);
            }
        }

        public async Task<ResponseDTO<IEnumerable<CategoryDTO>>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var response = new ResponseDTO<IEnumerable<CategoryDTO>>();

            var entities = await _categoryRepository.GetAsync(page, pageSize, cancellationToken).ConfigureAwait(false);
            var dtos = entities.Select(CategoryMapper.ToDto).ToList();

            response.Ok(dtos);
            return response;
        }

        public async Task<ResponseDTO<CategoryDTO>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var response = new ResponseDTO<CategoryDTO>();

            if (id == Guid.Empty)
            {
                response.Fail([], "Invalid id.");
                return response;
            }

            var entity = await _categoryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (entity is null)
            {
                response.Fail([], "Category not found.");
                return response;
            }

            response.Ok(CategoryMapper.ToDto(entity));
            return response;
        }

        public async Task<ResponseDTO<CategoryDTO>> UpdateAsync(Guid id, CategoryDTO dto, CancellationToken cancellationToken = default)
        {
            var response = new ResponseDTO<CategoryDTO>();

            if (id == Guid.Empty || dto is null)
            {
                response.Fail([], "Invalid input.");
                return response;
            }

            var existing = await _categoryRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (existing is null)
            {
                response.Fail([], "Category not found.");
                return response;
            }

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.UpdatedAt = DateTime.UtcNow;

            await _categoryRepository.UpdateAsync(existing, cancellationToken).ConfigureAwait(false);
            response.Ok(CategoryMapper.ToDto(existing));
            return response;
        }
    }
}
