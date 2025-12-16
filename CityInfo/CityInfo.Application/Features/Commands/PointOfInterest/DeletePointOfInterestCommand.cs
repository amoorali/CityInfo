using CityInfo.Application.Features.Results.PointOfInterest;
using MediatR;

namespace CityInfo.Application.Features.Commands.PointOfInterest
{
    public record DeletePointOfInterestCommand(
        int CityId,
        int PointOfInterestId
    ) : IRequest<DeletePointOfInterestResult>;
}
