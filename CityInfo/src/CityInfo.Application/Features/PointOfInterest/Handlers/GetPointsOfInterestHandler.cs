using CityInfo.Application.DTOs.PointOfInterest;
using CityInfo.Application.Features.PointOfInterest.Queries;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Repositories.Contracts;
using Mapster;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class GetPointsOfInterestHandler : IRequestHandler<GetPointsOfInterestQuery, GetPointsOfInterestResult>
    {
        #region [ Fields ]
        private readonly ICityRepository _cityRepository;
        private readonly IPointOfInterestRepository _pointOfInterestRepository;
        #endregion

        #region [ Constructor ]
        public GetPointsOfInterestHandler(
            ICityRepository cityRepository,
            IPointOfInterestRepository pointOfInterestRepository)
        {
            _cityRepository = cityRepository;
            _pointOfInterestRepository = pointOfInterestRepository;
        }
        #endregion

        #region [ Handler ]
        public async Task<GetPointsOfInterestResult> Handle(
            GetPointsOfInterestQuery request,
            CancellationToken cancellationToken)
        {
            if (!await _cityRepository.CityNameMatchesCityIdAsync(request.CityName, request.CityId))
                return new GetPointsOfInterestResult(true, true, null);

            if (!await _cityRepository.CityExistsAsync(request.CityId))
                return new GetPointsOfInterestResult(false, true, null);

            var pointsOfInterest = await _pointOfInterestRepository
                .GetPointsOfInterestForCityAsync(request.CityId);

            var entities = pointsOfInterest
                .Adapt<IReadOnlyList<PointOfInterestDto>>();

            return new GetPointsOfInterestResult(false, false, entities);
        }
        #endregion
    }
}
