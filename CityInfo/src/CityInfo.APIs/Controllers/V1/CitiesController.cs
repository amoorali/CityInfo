using Asp.Versioning;
using CityInfo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using MediatR;
using CityInfo.Application.Features.City.Queries;
using CityInfo.Application.Common.ResourceParameters;

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

        #region [ GET Methods ]
        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="cityId">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
        /// <returns>A city with or without points of interest</returns>
        /// <response code="200">Returns the requested city</response>
        /// <response code="404"></response>
        /// <response code="400"></response>
        [HttpGet("{cityId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCityAsync(
            int cityId, bool includePointsOfInterest = false)
        {
            var result = await _mediator.Send(new GetCityQuery(cityId, includePointsOfInterest));

            if (result.NotFound)
                return NotFound("The id isn't in the collection");

            if (includePointsOfInterest)
                return Ok(result.DtoWithPointsOfInterest);

            return Ok(result.DtoWithoutPointsOfInterest);
        }
        #endregion
    }
}
