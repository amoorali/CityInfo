using CityInfo.Application.Common.ResourceParameters;
using CityInfo.Application.Features.City.Results;
using MediatR;

namespace CityInfo.Application.Features.City.Queries
{
    public record GetCitiesQuery(
        CitiesResourceParameters CitiesResourceParameters,
        int PageNumber,
        int PageSize
    ) : IRequest<GetCitiesResult>;
}
