using Microsoft.EntityFrameworkCore;
using MySimpleStockProduct.Domain.Interfaces;

namespace MySimpleStockProduct.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(T product, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(T product, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IReadOnlyList<T>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            return await _context.Set<T>()
                .AsNoTracking()
                .OrderBy(c => EF.Property<Guid>(c, "Id"))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            T? result = await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
            return result;
        }

        public virtual async Task UpdateAsync(T product, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Update(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
