using Microsoft.EntityFrameworkCore;
using MySimpleStockProduct.Domain.Interfaces;
using MySimpleStockProduct.Logging;

namespace MySimpleStockProduct.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly ICustomLogger<BaseRepository<T>> _logger;

        public BaseRepository(DbContext context, ICustomLogger<BaseRepository<T>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual async Task AddAsync(T product, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Adding new record of type {0} to database", typeof(T).Name);

            try
            {
                await _context.Set<T>().AddAsync(product, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInfo("Record of type {0} successfully added to database", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add record of type {0} to database", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task DeleteAsync(T product, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Deleting record of type {0} from database", typeof(T).Name);

            try
            {
                _context.Set<T>().Remove(product);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInfo("Record of type {0} successfully deleted from database", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete record of type {0} from database", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<IReadOnlyList<T>> GetAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            _logger.LogInfo("Querying paged records for {0}. Page: {1}, PageSize: {2}", typeof(T).Name, page, pageSize);

            try
            {
                IReadOnlyList<T> results = await _context.Set<T>()
                    .AsNoTracking()
                    .OrderBy(c => EF.Property<Guid>(c, "Id"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                _logger.LogInfo("Retrieved {0} records of type {1} from database", results.Count, typeof(T).Name);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while querying paged records for {0}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Querying record of type {0} with ID: {1}", typeof(T).Name, id);

            try
            {
                T? result = await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);

                if (result is null)
                {
                    _logger.LogWarning("Record of type {0} with ID {1} was not found in database", typeof(T).Name, id);
                }
                else
                {
                    _logger.LogInfo("Record of type {0} with ID {1} successfully retrieved", typeof(T).Name, id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching record of type {0} with ID: {1}", typeof(T).Name, id);
                throw;
            }
        }

        public virtual async Task UpdateAsync(T product, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Updating record of type {0} in database", typeof(T).Name);

            try
            {
                _context.Set<T>().Update(product);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInfo("Record of type {0} successfully updated in database", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update record of type {0} in database", typeof(T).Name);
                throw;
            }
        }
    }
}
