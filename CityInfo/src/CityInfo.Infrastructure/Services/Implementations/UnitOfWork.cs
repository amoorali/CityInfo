using CityInfo.Application.Services.Contracts;
using CityInfo.Infrastructure.DbContexts;

namespace CityInfo.Infrastructure.Services.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        #region [ Fields ]
        private readonly CityInfoContext _context;
        #endregion

        #region [ Constructor ]
        public UnitOfWork(CityInfoContext context)
        {
            _context = context ?? 
                throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region [ Methods ]
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion
    }
}
