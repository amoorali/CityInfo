using CityInfo.Application.DTOs.City;

namespace CityInfo.Application.Features.City.Results
{
    public record GetCityResult(
        bool NotFound,
        CityDto? DtoWithPointsOfInterest,
        CityWithoutPointsOfInterestDto? DtoWithoutPointsOfInterest
    );
}
