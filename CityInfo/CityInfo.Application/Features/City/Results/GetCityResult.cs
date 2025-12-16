using CityInfo.Application.DTOs;

namespace CityInfo.Application.Features.City.Results
{
    public record GetCityResult(
        bool NotFound,
        CityDto? DtoWithPointsOfInterest,
        CityWithoutPointsOfInterestDto? DtoWithoutPointsOfInterest
    );
}
