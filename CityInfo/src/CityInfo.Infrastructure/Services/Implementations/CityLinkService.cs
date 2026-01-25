using CityInfo.Application.Common.Contracts;
using CityInfo.Application.Common.Helpers;
using CityInfo.Application.Common.ResourceParameters;
using CityInfo.Application.DTOs.Link;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace CityInfo.Infrastructure.Services.Implementations
{
    public class CityLinkService : ICityLinkService
    {
        #region [ Fields ]
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region [ Constructor ]
        public CityLinkService(
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region [ Private Methods ]
        private string? CreateCitiesResourceUri(
            CitiesResourceParameters citiesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, "GetCitiesAsync",
                        new
                        {
                            orderBy = citiesResourceParameters.OrderBy,
                            fields = citiesResourceParameters.Fields,
                            pageNumber = citiesResourceParameters.PageNumber - 1,
                            pageSize = citiesResourceParameters.PageSize,
                            name = citiesResourceParameters.Name,
                            searchQuery = citiesResourceParameters.SearchQuery
                        });
                case ResourceUriType.NextPage:
                    return _linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, "GetCitiesAsync",
                        new
                        {
                            orderBy = citiesResourceParameters.OrderBy,
                            fields = citiesResourceParameters.Fields,
                            pageNumber = citiesResourceParameters.PageNumber + 1,
                            pageSize = citiesResourceParameters.PageSize,
                            name = citiesResourceParameters.Name,
                            searchQuery = citiesResourceParameters.SearchQuery
                        });
                case ResourceUriType.Current:
                default:
                    return _linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, "GetCitiesAsync",
                        new
                        {
                            orderBy = citiesResourceParameters.OrderBy,
                            fields = citiesResourceParameters.Fields,
                            pageNumber = citiesResourceParameters.PageNumber + 1,
                            pageSize = citiesResourceParameters.PageSize,
                            name = citiesResourceParameters.Name,
                            searchQuery = citiesResourceParameters.SearchQuery
                        });
            }
        }
        #endregion

        public IEnumerable<LinkDto> CreateLinksForCity(int cityId, string? fields)
        {
            var links = new List<LinkDto>();

            object values = string.IsNullOrWhiteSpace(fields)
                ? new { cityId }
                : new { cityId, fields };

            links.Add(
                new(_linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, "GetCityAsync", values),
                "self",
                "GET"));

            links.Add(
                new(_linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, "CreatePointOfInterestAsync", new { cityId }),
                "create_pointofinterest_for_city",
                "POST"));

            links.Add(
                new(_linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, "GetPointsOfInterestAsync", new { cityId }),
                "pointsofinterest",
                "GET"));

            return links;
        }

        public IEnumerable<LinkDto> CreateLinksForCities(
            CitiesResourceParameters citiesResourceParameters,
            bool hasNext,
            bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new(CreateCitiesResourceUri(citiesResourceParameters, ResourceUriType.Current),
                "self",
                "GET")
            };

            if (hasNext)
            {
                links.Add(
                    new(CreateCitiesResourceUri(citiesResourceParameters, ResourceUriType.NextPage),
                    "nextPage",
                    "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new(CreateCitiesResourceUri(citiesResourceParameters, ResourceUriType.PreviousPage),
                    "previousPage",
                    "GET"));
            }

            return links;
        }
    }
}