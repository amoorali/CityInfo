using CityInfo.Application.Services.Contracts;
using MapsterMapper;

namespace CityInfo.Application.Features.Handlers.PointOfInterest
{
    public class GeneralHandler
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;
        protected readonly IMailService MailService;

        public GeneralHandler(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
        {
            UnitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            MailService = mailService ??
                throw new ArgumentNullException(nameof(mailService));
        }
    }
}
