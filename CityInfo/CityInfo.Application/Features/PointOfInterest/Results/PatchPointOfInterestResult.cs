using CityInfo.Application.DTOs;

namespace CityInfo.Application.Features.PointOfInterest.Results
{
    public record PatchPointOfInterestResult(
        bool CityNotFound,
        bool PointOfInterestNotFound,
        PointOfInterestForUpdateDto? DtoToValidate,
        Dictionary<string, string>? PatchErrors
    );
}
