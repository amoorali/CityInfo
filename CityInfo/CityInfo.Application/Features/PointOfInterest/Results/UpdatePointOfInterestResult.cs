using CityInfo.Application.DTOs;

namespace CityInfo.Application.Features.PointOfInterest.Results
{
    public record UpdatePointOfInterestResult(
        bool CityNotFound,
        bool PointOfInterestNotFound
    );
}
