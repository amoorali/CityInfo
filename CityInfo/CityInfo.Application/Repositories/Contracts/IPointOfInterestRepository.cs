using CityInfo.Domain.Entities;

namespace CityInfo.Application.Repositories.Contracts
{
    public interface IPointOfInterestRepository : IRepository<PointOfInterest>
    {
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
    }
}
