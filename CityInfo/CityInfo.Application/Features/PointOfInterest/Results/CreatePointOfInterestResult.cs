using CityInfo.Application.DTOs;

namespace CityInfo.Application.Features.PointOfInterest.Results
{
    public record CreatePointOfInterestResult(
        bool CityNotFound, 
        PointOfInterestDto? Created
    );
}
