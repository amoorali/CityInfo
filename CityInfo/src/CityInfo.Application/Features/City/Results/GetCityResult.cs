using CityInfo.Application.DTOs.City;

namespace CityInfo.Application.Features.City.Results
{
    #region [ Result Record ]
    public record GetCityResult(
        bool NotFound,
        CityDto? Dto,
        IDictionary<string, object?>? LinkedResources
    );
    #endregion
}
