namespace MySimpleStockProduct.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default);

        Task AddAsync(T product, CancellationToken cancellationToken = default);

        Task UpdateAsync(T product, CancellationToken cancellationToken = default);

        Task DeleteAsync(T product, CancellationToken cancellationToken = default);
    }
}
