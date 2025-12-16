using CityInfo.Application.DTOs;

namespace CityInfo.Application.Features.Results.PointOfInterest
{
    public record UpdatePointOfInterestResult(
        bool CityNotFound,
        bool PointOfInterestNotFound
    );
}
