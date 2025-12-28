using CityInfo.Application.Common.Helpers;
using CityInfo.Application.Common.ResourceParameters;
using CityInfo.Domain.Entities;

namespace CityInfo.Application.Repositories.Contracts
{
    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<PagedList<City>> GetCitiesAsync(CitiesResourceParameters citiesResourceParameters);
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        Task<bool> CityExistsAsync(int cityId);
        Task<bool> CityNameMatchesCityIdAsync(string? cityName, int cityId);
    }
}
