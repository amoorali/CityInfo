using CityInfo.Application.Features.Commands.PointOfInterest;
using CityInfo.Application.Features.Results.PointOfInterest;
using CityInfo.Application.Services.Contracts;
using MapsterMapper;
using MediatR;

namespace CityInfo.Application.Features.Handlers.PointOfInterest
{
    public class UpdatePointOfInterestHandler : GeneralHandler,
        IRequestHandler<UpdatePointOfInterestCommand, UpdatePointOfInterestResult>
    {
        public UpdatePointOfInterestHandler(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<UpdatePointOfInterestResult> Handle(
            UpdatePointOfInterestCommand request,
            CancellationToken cancellationToken)
        {
            if (!await UnitOfWork.Cities.CityExistsAsync(request.CityId))
                return new UpdatePointOfInterestResult(false, false);

            var entity = await UnitOfWork.PointsOfInterest
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (entity == null)
                return new UpdatePointOfInterestResult(true, false);

            Mapper.Map(request.Dto, entity);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdatePointOfInterestResult(true, true);
        }
    }
}
