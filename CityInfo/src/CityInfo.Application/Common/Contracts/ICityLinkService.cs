using CityInfo.Application.Common.ResourceParameters;
using CityInfo.Application.DTOs.Link;

namespace CityInfo.Application.Common.Contracts
{
    public interface ICityLinkService
    {
        IEnumerable<LinkDto> CreateLinksForCity(int cityId, string? fields);
        IEnumerable<LinkDto> CreateLinksForCities(
            CitiesResourceParameters parameters,
            bool hasNext,
            bool hasPrevious);
    }
}