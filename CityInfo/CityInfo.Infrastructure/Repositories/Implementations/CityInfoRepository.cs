using CityInfo.Application.Common;
using CityInfo.Domain.Entities;
using CityInfo.Infrastructure.DbContexts;
using CityInfo.Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CityInfo.Infrastructure.Repositories.Implementations
{

    public class CityInfoRepository : ICityInfoRepository
    {
        #region [ Fields ]
        internal readonly CityInfoContext _context;
        #endregion

        #region [ Cosntructure ]
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region [ Database Methods ]
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
        #endregion
    }
}
