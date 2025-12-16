using CityInfo.Application.DTOs;
using CityInfo.Application.Features.Results.PointOfInterest;
using MediatR;

namespace CityInfo.Application.Features.Commands.PointOfInterest
{
    public record UpdatePointOfInterestCommand(
        int CityId,
        int PointOfInterestId,
        PointOfInterestForUpdateDto Dto
    ) : IRequest<UpdatePointOfInterestResult>;
}
