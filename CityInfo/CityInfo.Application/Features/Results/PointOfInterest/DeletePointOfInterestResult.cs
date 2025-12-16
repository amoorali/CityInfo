namespace CityInfo.Application.Features.Results.PointOfInterest
{
    public record DeletePointOfInterestResult(
        bool CityNotFound,
        bool PointOfInterestNotFound
    );
}
