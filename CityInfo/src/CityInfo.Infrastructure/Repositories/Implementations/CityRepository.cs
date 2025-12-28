using CityInfo.Application.Common;
using CityInfo.Application.Common.Helpers;
using CityInfo.Application.Common.ResourceParameters;
using CityInfo.Application.Repositories.Contracts;
using CityInfo.Domain.Entities;
using CityInfo.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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

        #region [ City Methods ]
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await Context.Cities
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<PagedList<City>> GetCitiesAsync(CitiesResourceParameters citiesResourceParameters)
        {
            // collection to start from
            var collection = Context.Cities.AsQueryable();

            if (!string.IsNullOrEmpty(citiesResourceParameters.Name))
            {
                citiesResourceParameters.Name = citiesResourceParameters.Name.Trim();
                collection = collection.Where(c => c.Name == citiesResourceParameters.Name);
            }

            if (!string.IsNullOrWhiteSpace(citiesResourceParameters.SearchQuery))
            {
                citiesResourceParameters.SearchQuery = citiesResourceParameters.SearchQuery.Trim();
                collection = collection.Where(c => c.Name.Contains(citiesResourceParameters.SearchQuery)
                || !string.IsNullOrEmpty(c.Description) && c.Description.Contains(citiesResourceParameters.SearchQuery));
            }

            return await PagedList<City>.CreateAsync(
                collection,
                citiesResourceParameters.PageNumber,
                citiesResourceParameters.PageSize);
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await Context.Cities
                    .Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId)
                    .FirstOrDefaultAsync();
            }

            return await Context.Cities
                .Where(c => c.Id == cityId)
                .FirstOrDefaultAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);

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

        public CityInfoContext CityInfoContext
        {
            get { return Context as CityInfoContext; }
        }
    }
}
