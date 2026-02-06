using Asp.Versioning;
using CityInfo.Application.Common.ResourceParameters;
using CityInfo.Application.DTOs.City;
using CityInfo.Application.Features.City.Commands;
using CityInfo.Application.Features.City.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.APIs.Controllers.V1
{
    [ApiController]
    //[Authorize]
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

        #region [ GET Methods ]
        [HttpGet(Name = "GetCitiesAsync")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCitiesAsync(
            [FromQuery] CitiesResourceParameters citiesResourceParameters,
            [FromHeader(Name = "Accept")] string? mediaType)
        {
            var result = await _mediator.Send(new GetCitiesQuery(citiesResourceParameters, mediaType));

            Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(result.PaginationMetadata));

            return Ok(result.Item);
        }

        /// <summary>
        /// Get a city by id
        /// </summary>
        ///<param name="cityId">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
        /// <param name="fields">The fields for city to return</param>
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
            string? fields,
            [FromHeader(Name = "Accept")] string? mediaType)
        {
            var result = await _mediator.Send(new GetCityQuery(
                cityId,
                includePointsOfInterest,
                fields,
                mediaType));

            if (result.NotFound)
                return NotFound("The id isn't in the collection");

            return Ok(result.Item);
        }
        #endregion

        #region [ POST Methods ]
        [HttpPost(Name = "CreateCityAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCityAsync(
            CityForCreationDto city,
            [FromHeader(Name = "Accept")] string? mediaType)
        {
            var result = await _mediator.Send(new CreateCityCommand(city, mediaType));

            return CreatedAtRoute("GetCityAsync", result.Item);
        }
        #endregion
    }
}
