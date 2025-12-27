using CityInfo.Application.DTOs.PointOfInterest;

namespace CityInfo.Application.Features.PointOfInterest.Results
{
    public record CreatePointOfInterestResult(
        bool CityNotFound, 
        PointOfInterestDto? Created
    );
}
