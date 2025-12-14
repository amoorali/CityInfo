using CityInfo.Application.Common;
using CityInfo.Domain.Entities;

namespace CityInfo.Infrastructure.Repositories.Contracts
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        Task<bool> CityExistsAsync(int cityId);
        Task<bool> CityNameMatchesCityIdAsync(string? cityName, int cityId);
    }
}
