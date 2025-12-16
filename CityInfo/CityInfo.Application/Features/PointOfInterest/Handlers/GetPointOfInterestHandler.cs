using CityInfo.Application.DTOs;
using CityInfo.Application.Features.BaseImplementations;
using CityInfo.Application.Features.PointOfInterest.Queries;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Services.Contracts;
using MapsterMapper;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class GetPointOfInterestHandler : GeneralHandler,
        IRequestHandler<GetPointOfInterestQuery, GetPointOfInterestResult>
    {
        public GetPointOfInterestHandler(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
            : base(unitOfWork, mapper, mailService)
        {
        }

        public async Task<GetPointOfInterestResult> Handle(
            GetPointOfInterestQuery request,
            CancellationToken cancellationToken)
        {
            if (!await UnitOfWork.Cities.CityExistsAsync(request.CityId))
                return new GetPointOfInterestResult(true, true, null);

            var pointOfInterest = await UnitOfWork.PointsOfInterest
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (pointOfInterest == null)
                return new GetPointOfInterestResult(false, true, null);

            var dto = Mapper.Map<PointOfInterestDto>(pointOfInterest);

            return new GetPointOfInterestResult(false, false, dto);
        }
    }
}
