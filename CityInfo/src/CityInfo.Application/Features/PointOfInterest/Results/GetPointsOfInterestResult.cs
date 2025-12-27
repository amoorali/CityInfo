using CityInfo.Application.DTOs.PointOfInterest;

namespace CityInfo.Application.Features.PointOfInterest.Results
{
    public record GetPointsOfInterestResult(
        bool Forbid,
        bool CityNotFound,
        IReadOnlyList<PointOfInterestDto>? Items
    );
}
