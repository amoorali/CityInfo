using MapsterMapper;
using CityInfo.Application.Services.Contracts;
using MediatR;
using CityInfo.Application.DTOs;
using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Results;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class CreatePointOfInterestHandler : GeneralHandler,
        IRequestHandler<CreatePointOfInterestCommand, CreatePointOfInterestResult>
    {
        public CreatePointOfInterestHandler(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
            : base(unitOfWork, mapper, mailService)
        {
        }

        public async Task<CreatePointOfInterestResult> Handle(
            CreatePointOfInterestCommand request,
            CancellationToken cancellationToken)
        {
            if (!await UnitOfWork.Cities.CityExistsAsync(request.CityId))
                return new CreatePointOfInterestResult(true, null);

            var entity = Mapper.Map<Domain.Entities.PointOfInterest>(request.Dto);

            await UnitOfWork.Cities.AddPointOfInterestForCityAsync(request.CityId, entity);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            var createdDto = Mapper.Map<PointOfInterestDto>(entity);

            return new CreatePointOfInterestResult(false, createdDto);
        }
    }
}
