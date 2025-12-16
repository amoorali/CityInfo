using CityInfo.Application.Features.City.Results;
using MediatR;

namespace CityInfo.Application.Features.City.Queries
{
    public record GetCitiesQuery(
        string? Name,
        string? SearchQuery,
        int PageNumber,
        int PageSize
    ) : IRequest<GetCitiesResult>;
}
