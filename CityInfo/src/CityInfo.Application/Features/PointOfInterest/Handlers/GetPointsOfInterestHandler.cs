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
        private readonly IPointOfInterestRepository _pointOfInterestRepository;
        #endregion

        #region [ Constructor ]
        public GetPointsOfInterestHandler(
            IPointOfInterestRepository pointOfInterestRepository)
        {
            _pointOfInterestRepository = pointOfInterestRepository;
        }
        #endregion

        #region [ Handler ]
        public async Task<GetPointsOfInterestResult> Handle(
            GetPointsOfInterestQuery request,
            CancellationToken cancellationToken)
        {
            var pointsOfInterest = await _pointOfInterestRepository
                .GetPointsOfInterestForCityAsync(request.CityId);

            var entities = pointsOfInterest
                .Adapt<IReadOnlyList<PointOfInterestDto>>();

            return new GetPointsOfInterestResult(false, entities);
        }
        #endregion
    }
}
