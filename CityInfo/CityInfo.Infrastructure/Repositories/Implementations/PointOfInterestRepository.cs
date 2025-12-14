using CityInfo.Domain.Entities;
using CityInfo.Infrastructure.DbContexts;
using CityInfo.Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Infrastructure.Repositories.Implementations
{
    public class PointOfInterestRepository : GeneralRepository, IPointOfInterestRepository
    {
        #region [ Constructor ]
        public PointOfInterestRepository(CityInfoContext _context) : base(_context)
        {
        }
        #endregion

        #region [ PointOfInterest Methods ]
        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointsOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointsOfInterest
                .Where(p => p.CityId == cityId)
                .ToListAsync();
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }
        #endregion
    }
}
