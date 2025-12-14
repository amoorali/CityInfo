using CityInfo.Infrastructure.DbContexts;
using CityInfo.Infrastructure.Services.Contracts;

namespace CityInfo.Infrastructure.Services.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CityInfoContext _context;

        public UnitOfWork(CityInfoContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _context.SaveChangesAsync(cancellationToken);
    }
}
