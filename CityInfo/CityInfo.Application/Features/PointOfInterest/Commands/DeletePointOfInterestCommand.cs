using CityInfo.Application.Features.PointOfInterest.Results;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Commands
{
    public record DeletePointOfInterestCommand(
        int CityId,
        int PointOfInterestId
    ) : IRequest<DeletePointOfInterestResult>;
}
