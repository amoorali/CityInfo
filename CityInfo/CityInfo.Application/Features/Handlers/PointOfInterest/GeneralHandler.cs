using CityInfo.Application.Services.Contracts;
using MapsterMapper;

namespace CityInfo.Application.Features.Handlers.PointOfInterest
{
    public class GeneralHandler
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;

        public GeneralHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
    }
}
