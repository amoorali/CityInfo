using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Services.Contracts;
using MapsterMapper;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class UpdatePointOfInterestHandler : GeneralHandler,
        IRequestHandler<UpdatePointOfInterestCommand, UpdatePointOfInterestResult>
    {
        public UpdatePointOfInterestHandler(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
            : base(unitOfWork, mapper, mailService)
        {
        }

        public async Task<UpdatePointOfInterestResult> Handle(
            UpdatePointOfInterestCommand request,
            CancellationToken cancellationToken)
        {
            if (!await UnitOfWork.Cities.CityExistsAsync(request.CityId))
                return new UpdatePointOfInterestResult(true, true);

            var entity = await UnitOfWork.PointsOfInterest
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (entity == null)
                return new UpdatePointOfInterestResult(false, true);

            Mapper.Map(request.Dto, entity);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdatePointOfInterestResult(false, false);
        }
    }
}
