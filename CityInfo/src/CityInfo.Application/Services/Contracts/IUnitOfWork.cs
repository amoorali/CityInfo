using CityInfo.Application.Repositories.Contracts;

namespace CityInfo.Application.Services.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
