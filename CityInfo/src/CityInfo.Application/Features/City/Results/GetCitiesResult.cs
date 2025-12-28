using CityInfo.Application.Common;
using CityInfo.Application.DTOs.City;

namespace CityInfo.Application.Features.City.Results
{
    public record GetCitiesResult(
        IReadOnlyList<CityWithoutPointsOfInterestDto> Items,
        bool HasPreviousPage,
        bool HasNextPage,
        PaginationMetadata PaginationMetadata
    );
}
