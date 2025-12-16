using Asp.Versioning;
using CityInfo.Application.DTOs;
using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.APIs.Controllers.V1
{
    [ApiController]
    [Authorize(Policy = "MustBeFromAntwerp")]
    [ApiVersion(1)]
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

        #region [ POST Methods ]
        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterest)
        {
            var result = await _mediator.Send(new CreatePointOfInterestCommand(cityId, pointOfInterest));

            if (result.CityNotFound)
                return NotFound("City not found!");

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId,
                    pointOfInterestId = result.Created.Id
                },
                result.Created);
        }
        #endregion

        #region [ PUT Methods ]
        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
            var result = await _mediator.Send(new UpdatePointOfInterestCommand(cityId, pointOfInterestId, pointOfInterest));

            if(result.CityNotFound)
                return NotFound("City not found!");

            else if (result.PointOfInterestNotFound)
                return NotFound($"Point of Interest of city with cityId {cityId} was not found to create!");

            return NoContent();
        }
        #endregion

        #region [ PATCH Methods ]
        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var result = await _mediator.Send(new PatchPointOfInterestCommand(cityId, pointOfInterestId, patchDocument));

            if (result.CityNotFound)
                return NotFound("City not found!");

            if (result.PointOfInterestNotFound)
                return NotFound($"Point of Interest of city with cityId {cityId} was not found to update!");

            if (result.PatchErrors is not null)
            {
                foreach (var error in result.PatchErrors)
                    ModelState.AddModelError(error.Key, error.Value);

                return BadRequest(ModelState);
            }

            if (!TryValidateModel(result.DtoToValidate!))
                return BadRequest(ModelState);

            return NoContent();
        }
        #endregion

        #region [ DELETE Methods ]
        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            var result = await _mediator.Send(new DeletePointOfInterestCommand(cityId, pointOfInterestId));
            
            if (result.CityNotFound)
                return NotFound("City not found!");

            if (result.PointOfInterestNotFound)
                return NotFound($"Point of Interest of city with cityId {cityId} was not found to delete!");

            return NoContent();
        }
        #endregion
    }
}
