using CityInfo.Application.Common;
using CityInfo.Domain.Entities;
using CityInfo.Infrastructure.DbContexts;
using CityInfo.Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CityInfo.Infrastructure.Repositories.Implementations
{

    public class GeneralRepository
    {
        #region [ Fields ]
        protected readonly CityInfoContext _context;
        #endregion

        #region [ Cosntructure ]
        public GeneralRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion
    }
}
