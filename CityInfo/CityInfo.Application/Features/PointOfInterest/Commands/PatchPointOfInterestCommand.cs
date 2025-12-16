using CityInfo.Application.DTOs;
using CityInfo.Application.Features.PointOfInterest.Results;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace CityInfo.Application.Features.PointOfInterest.Commands
{
    public record PatchPointOfInterestCommand(
        int CityId,
        int PointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> PatchDocument
    ) : IRequest<PatchPointOfInterestResult>;
}
