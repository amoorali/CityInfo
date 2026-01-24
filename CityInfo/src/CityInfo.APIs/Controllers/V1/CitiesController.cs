using Asp.Versioning;
using CityInfo.Application.Common.Helpers;
using CityInfo.Application.Common.ResourceParameters;
using CityInfo.Application.Features.City.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.APIs.Controllers.V1
{
    [ApiController]
    [Authorize]
    [ApiVersion(1, Deprecated = true)]
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

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(result.PaginationMetadata));

            return Ok(result.Items);
        }

        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="cityId">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
        /// <param name="fields">Specify the fields to retrieve</param>
        /// <returns>A city with or without points of interest</returns>
        /// <response code="200">Returns the requested city</response>
        /// <response code="404"></response>
        /// <response code="400"></response>
        [HttpGet("{cityId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCityAsync(
            int cityId,
            string? fields,
            bool includePointsOfInterest = false)
        {
            var result = await _mediator.Send(new GetCityQuery(cityId, includePointsOfInterest, fields));

            if (result.NotFound)
                return NotFound("The id isn't in the collection");

            return Ok(result.Item);
        }
        #endregion
    }
}
