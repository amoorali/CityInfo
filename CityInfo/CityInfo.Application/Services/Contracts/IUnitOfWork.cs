using CityInfo.Application.Repositories.Contracts;

namespace CityInfo.Application.Services.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        #region [ Fields ]
        ICityRepository Cities { get; }
        IPointOfInterestRepository PointsOfInterest { get; }
        #endregion

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
