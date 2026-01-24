using CityInfo.Application.DTOs.Link;
using CityInfo.Application.Features.City.Results;
using MediatR;

namespace CityInfo.Application.Features.City.Queries
{
    #region [ Query Record ]
    public record GetCityQuery(
        int CityId,
        bool IncludePointsOfInterest,
        string? Fields,
        IEnumerable<LinkDto> Links,
        string? MediaType
    ) : IRequest<GetCityResult>;
    #endregion
}

