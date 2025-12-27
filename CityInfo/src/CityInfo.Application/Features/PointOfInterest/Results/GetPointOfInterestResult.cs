using CityInfo.Application.DTOs.PointOfInterest;

namespace CityInfo.Application.Features.PointOfInterest.Results
{
    public record GetPointOfInterestResult(
        bool CityNotFound,
        bool PointOfInterestNotFound,
        PointOfInterestDto? Item
    );
}
