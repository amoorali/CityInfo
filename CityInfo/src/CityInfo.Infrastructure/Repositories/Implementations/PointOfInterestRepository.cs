using CityInfo.Application.Repositories.Contracts;
using CityInfo.Domain.Entities;
using CityInfo.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Infrastructure.Repositories.Implementations
{
    public class PointOfInterestRepository : Repository<PointOfInterest>, IPointOfInterestRepository
    {
        #region [ Constructor ]
        public PointOfInterestRepository(CityInfoContext context)
            : base(context)
        {
        }
        #endregion

        #region [ Methods ]
        public Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return Context.PointsOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await Context.PointsOfInterest
                .Where(p => p.CityId == cityId)
                .ToListAsync();
        }

        public async Task AddPointOfInterestForCityAsync(PointOfInterest pointOfInterest)
        {
            await Context.Set<PointOfInterest>().AddAsync(pointOfInterest).ConfigureAwait(false);
        }

        public async Task<bool> CityNameMatchesCityIdAsync(string? cityName, int cityId)
        {
            return await Context.PointsOfInterest.AnyAsync(p => p.CityId == cityId && p.City!.Name == cityName);
        }
        #endregion
    }
}
