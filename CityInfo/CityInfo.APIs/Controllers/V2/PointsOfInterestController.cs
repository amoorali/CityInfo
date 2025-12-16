using Asp.Versioning;
using CityInfo.Application.DTOs;
using CityInfo.Application.Features.PointOfInterest.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.APIs.Controllers.V2
{
    [ApiController]
    [Authorize(Policy = "MustBeFromAntwerp")]
    [ApiVersion(2)]
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        #region [ Fields ]
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMediator _mediator;
        #endregion

        #region [ Constructure ]
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMediator mediator)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }
        #endregion

        #region [ GET Methods ]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterestAsync(int cityId)
        {
            var cityName = User.Claims.First(c => c.Type == "city").Value;

            var result = await _mediator.Send(new GetPointsOfInterestQuery(cityId, cityName));

            if (result.Forbid)
                return Forbid();

            else if (result.CityNotFound)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");

                return NotFound("City not found");
            }

            return Ok(result.Items);

        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            var result = await _mediator.Send(new GetPointOfInterestQuery(cityId, pointOfInterestId));

            if (result.CityNotFound)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");

                return NotFound("City not found");
            }

            if (result.PointOfInterestNotFound)
                return NotFound("Point of interest not found");

            return Ok(result.Item);
        }
        #endregion
    }
}
