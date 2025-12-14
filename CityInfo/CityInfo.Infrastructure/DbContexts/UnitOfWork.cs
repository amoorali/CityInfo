namespace CityInfo.Infrastructure.DbContexts
{
    public class UnitOfWork
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
