using CityInfo.Application.DTOs;
using CityInfo.Application.Features.PointOfInterest.Results;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Commands
{
    public record UpdatePointOfInterestCommand(
        int CityId,
        int PointOfInterestId,
        PointOfInterestForUpdateDto Dto
    ) : IRequest<UpdatePointOfInterestResult>;
}
