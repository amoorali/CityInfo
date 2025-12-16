using CityInfo.Application.Features.City.Results;
using MediatR;

namespace CityInfo.Application.Features.City.Queries
{
    public record GetCityQuery(
        int CityId,
        bool IncludePointsOfInterest
    ) : IRequest<GetCityResult>;
}

