using CityInfo.Application.DTOs.PointOfInterest;
using CityInfo.Application.Features.PointOfInterest.Queries;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Repositories.Contracts;
using Mapster;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class GetPointOfInterestHandler : IRequestHandler<GetPointOfInterestQuery, GetPointOfInterestResult>
    {
        #region [ Fields ]
        private readonly ICityRepository _cityRepository;
        private readonly IPointOfInterestRepository _pointOfInterestRepository;
        #endregion

        #region [ Constructor ]
        public GetPointOfInterestHandler(
            ICityRepository cityRepository,
            IPointOfInterestRepository pointOfInterestRepository)
        {
            _cityRepository = cityRepository;
            _pointOfInterestRepository = pointOfInterestRepository;
        }
        #endregion

        #region [ Handler ]
        public async Task<GetPointOfInterestResult> Handle(
            GetPointOfInterestQuery request,
            CancellationToken cancellationToken)
        {
            if (!await _cityRepository.CityExistsAsync(request.CityId))
                return new GetPointOfInterestResult(true, true, null);

            var pointOfInterest = await _pointOfInterestRepository
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (pointOfInterest == null)
                return new GetPointOfInterestResult(false, true, null);

            var dto = pointOfInterest
                .Adapt<PointOfInterestDto>();

            return new GetPointOfInterestResult(false, false, dto);
        }
        #endregion
    }
}
