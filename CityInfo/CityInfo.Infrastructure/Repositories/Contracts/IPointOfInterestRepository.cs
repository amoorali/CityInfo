using CityInfo.Domain.Entities;

namespace CityInfo.Infrastructure.Repositories.Contracts
{
    public interface IPointOfInterestRepository
    {
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
    }
}
