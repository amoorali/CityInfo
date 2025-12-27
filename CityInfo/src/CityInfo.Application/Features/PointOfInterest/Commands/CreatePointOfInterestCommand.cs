using CityInfo.Application.DTOs.PointOfInterest;
using CityInfo.Application.Features.PointOfInterest.Results;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Commands
{
    public record CreatePointOfInterestCommand (
        int CityId,
        PointOfInterestForCreationDto Dto
    ) : IRequest<CreatePointOfInterestResult>;
}
