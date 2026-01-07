using CityInfo.Application.Repositories.Contracts;
using CityInfo.Domain.Entities;
using CityInfo.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Infrastructure.Repositories.Implementations
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        #region [ Constructor ]
        public CityRepository(CityInfoContext context)
            : base(context)
        {
        }
        #endregion

        #region [ Methods ]
        public IQueryable<City> QueryCities()
            => Context.Cities.AsNoTracking();

        public async Task<City?> GetCityWithPointsOfInterestAsync(int cityId)
        {
            return await Context.Cities
                .Include(c => c.PointsOfInterest)
                .FirstOrDefaultAsync(c => c.Id == cityId);
        }

        public async Task<City?> GetCityWithoutPointsOfInterestAsync(int cityId)
        {
            return await Context.Cities
                .FirstOrDefaultAsync(c => c.Id == cityId);
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityWithoutPointsOfInterestAsync(cityId);

            if (city != null)
                city.PointsOfInterest.Add(pointOfInterest);
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await Context.Cities.AnyAsync(c => c.Id == cityId);
        }

        #region [ Bool Expression Methods ]
        public async Task<bool> CityNameMatchesCityIdAsync(string? cityName, int cityId)
        {
            return await Context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        }
        #endregion

        #endregion
    }
}
