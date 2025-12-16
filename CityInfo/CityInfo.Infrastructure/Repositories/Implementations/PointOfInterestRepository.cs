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

        #region [ PointOfInterest Methods ]
        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await Context.PointsOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await Context.PointsOfInterest
                .Where(p => p.CityId == cityId)
                .ToListAsync();
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            Context.PointsOfInterest.Remove(pointOfInterest);
        }
        #endregion
    }
}
