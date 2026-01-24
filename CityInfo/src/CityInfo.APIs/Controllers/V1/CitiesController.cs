using Asp.Versioning;
using CityInfo.Application.Common.Helpers;
using CityInfo.Application.Common.ResourceParameters;
using CityInfo.Application.DTOs.City;
using CityInfo.Application.DTOs.Link;
using CityInfo.Application.Features.City.Commands;
using CityInfo.Application.Features.City.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.APIs.Controllers.V1
{
    [ApiController]
    [Authorize]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/cities")]
    public class CitiesController : ControllerBase
    {
        #region [ Fields ]
        private readonly IMediator _mediator;
        #endregion

        #region [ Constructor ]
        public CitiesController(IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
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
                    return Url.Link("GetCitiesAsync",
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
                    return Url.Link("GetCitiesAsync",
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
                    return Url.Link("GetCitiesAsync",
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

        private IEnumerable<LinkDto> CreateLinksForCities(
            CitiesResourceParameters citiesResourceParameters)
        {
            var links = new List<LinkDto>();

            links.Add(
                new(CreateCitiesResourceUri(citiesResourceParameters, ResourceUriType.Current),
                "self",
                "GET"));

            throw new NotImplementedException();

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForCity(int cityId, string? fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new(Url.Link("GetCityAsync", new { cityId }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(
                    new(Url.Link("GetCityAsync", new { cityId, fields }),
                    "self",
                    "GET"));
            }

            links.Add(
                new(Url.Link("CreatePointOfInterestAsync", new { cityId }),
                "create_pointofinterest_for_city",
                "POST"));

            links.Add(
                new(Url.Link("GetPointsOfInterestAsync", new { cityId }),
                "pointsofinterest",
                "GET"));

            return links;
        }
        #endregion

        #region [ GET Methods ]
        [HttpGet(Name = "GetCitiesAsync")]
        [HttpHead]
        public async Task<ActionResult> GetCitiesAsync(
            [FromQuery] CitiesResourceParameters citiesResourceParameters)
        {
            var result = await _mediator.Send(new GetCitiesQuery(citiesResourceParameters));

            var previousPageLink = result.HasPreviousPage
                ? CreateCitiesResourceUri(
                    citiesResourceParameters,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = result.HasNextPage
                ? CreateCitiesResourceUri(
                    citiesResourceParameters,
                    ResourceUriType.NextPage) : null;

            result.PaginationMetadata.PreviousPageLink = previousPageLink;
            result.PaginationMetadata.NextPageLink = nextPageLink;

            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(result.PaginationMetadata));

            return Ok(result.Items);
        }

        /// <summary>
        /// Get a city by id
        /// </summary>
        ///<param name="cityId">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
        /// <param name="citiesResourceParameters">The resource parameters for city to query</param>
        /// <param name="mediaType">The type of output media</param>
        /// <returns>A city with or without points of interest</returns>
        /// <response code="200">Returns the requested city</response>
        /// <response code="404"></response>
        /// <response code="400"></response>
        [HttpGet("{cityId}", Name = "GetCityAsync")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCityAsync(
            int cityId,
            bool includePointsOfInterest,
            [FromQuery] CitiesResourceParameters citiesResourceParameters,
            [FromHeader(Name = "Accept")] string? mediaType)
        {

            var links = CreateLinksForCity(cityId, citiesResourceParameters.Fields);

            var result = await _mediator.Send(new GetCityQuery(
                cityId,
                includePointsOfInterest,
                citiesResourceParameters.Fields,
                links,
                mediaType));

            if (result.NotFound)
                return NotFound("The id isn't in the collection");

            return Ok(result.LinkedResources);
        }
        #endregion

        #region [ POST Methods ]
        [HttpPost]
        public async Task<IActionResult> CreateCityAsync(
            CityForCreationDto city)
        {
            throw new NotImplementedException();

            var links = CreateLinksForCity(1, null);

            var result = await _mediator.Send(new CreateCityCommand(city, links));

            return CreatedAtRoute("GetCityAsync",
                new { cityId = result.LinkedResources!["Id"] },
                result.LinkedResources);
        }
        #endregion
    }
}
