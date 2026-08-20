using MySimpleStockProduct.Application.DTOs;

namespace MySimpleStockProduct.Application.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<ResponseDTO<T>> CreateAsync(T dto, CancellationToken cancellationToken = default);
        Task<ResponseDTO<T>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ResponseDTO<IEnumerable<T>>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<ResponseDTO<T>> UpdateAsync(Guid id, T dto, CancellationToken cancellationToken = default);
        Task<ResponseDTO<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
