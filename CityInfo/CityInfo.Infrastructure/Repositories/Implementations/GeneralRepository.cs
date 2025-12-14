using CityInfo.Infrastructure.DbContexts;

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
