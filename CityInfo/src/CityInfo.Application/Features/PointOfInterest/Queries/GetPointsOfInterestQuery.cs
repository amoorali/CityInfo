using CityInfo.Application.Common.Contracts;
using CityInfo.Application.Features.PointOfInterest.Results;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Queries
{
    #region [ Query Record ]
    public record GetPointsOfInterestQuery(
        int CityId
    ) : IRequest<GetPointsOfInterestResult>, IRequireExisitingCity;
    #endregion
}
