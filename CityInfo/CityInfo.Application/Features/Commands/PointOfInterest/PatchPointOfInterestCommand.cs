using CityInfo.Application.DTOs;
using CityInfo.Application.Features.Results.PointOfInterest;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace CityInfo.Application.Features.Commands.PointOfInterest
{
    public record PatchPointOfInterestCommand(
        int CityId,
        int PointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> PatchDocument
    ) : IRequest<PatchPointOfInterestResult>;
}
