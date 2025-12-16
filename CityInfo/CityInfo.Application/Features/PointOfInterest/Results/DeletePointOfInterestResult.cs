namespace CityInfo.Application.Features.PointOfInterest.Results
{
    public record DeletePointOfInterestResult(
        bool CityNotFound,
        bool PointOfInterestNotFound
    );
}
