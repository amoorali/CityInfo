using Asp.Versioning;
using CityInfo.Application.DTOs.PointOfInterest;
using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace CityInfo.APIs.Controllers.V1
{
    [ApiController]
    [Authorize(Policy = "MustBeFromAntwerp")]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        #region [ Fields ]
        private readonly IMediator _mediator;
        #endregion

        #region [ Constructor ]
        public PointsOfInterestController(
            IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }
        #endregion

        #region [ GET Methods ]
        [HttpGet(Name = "GetPointsOfInterestAsync")]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterestAsync(int cityId)
        {
            var cityName = User.Claims.First(c => c.Type == "city").Value;

            var result = await _mediator.Send(new GetPointsOfInterestQuery(cityId, cityName));

            if (result.Forbid)
                return Forbid();

            return Ok(result.Items);
            
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterestAsync")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            var result = await _mediator.Send(new GetPointOfInterestQuery(cityId, pointOfInterestId));

            if (result.PointOfInterestNotFound)
                return NotFound("Point of interest not found");

            return Ok(result.Item);
        }
        #endregion

        #region [ POST Methods ]
        [HttpPost(Name = "CreatePointOfInterestAsync")]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterestAsync(
            int cityId,
            PointOfInterestForCreationDto pointOfInterest)
        {
            var result = await _mediator.Send(new CreatePointOfInterestCommand(cityId, pointOfInterest));

            return CreatedAtRoute("GetPointOfInterestAsync",
                new
                {
                    cityId,
                    pointOfInterestId = result.Created.Id
                },
                result.Created);
        }
        #endregion

        #region [ PUT Methods ]
        [HttpPut("{pointOfInterestId}", Name = "UpdatePointOfInterestAsync")]
        public async Task<ActionResult> UpdatePointOfInterestAsync(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
            var result = await _mediator.Send(new UpdatePointOfInterestCommand(cityId, pointOfInterestId, pointOfInterest));

            if (result.PointOfInterestNotFound)
                return NotFound($"Point of Interest of city with cityId {cityId} was not found to create!");

            return NoContent();
        }
        #endregion

        #region [ PATCH Methods ]
        [HttpPatch("{pointOfInterestId}", Name = "PartiallyUpdatePointOfInterestAsync")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterestAsync(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var result = await _mediator.Send(new PatchPointOfInterestCommand(cityId, pointOfInterestId, patchDocument));

            if (result.PointOfInterestNotFound)
                return NotFound($"Point of Interest of city with cityId {cityId} was not found to update!");

            if (result.PatchErrors is not null)
            {

                foreach (var error in result.PatchErrors)
                    ModelState.AddModelError(error.Key, error.Value);

                return ValidationProblem(ModelState);
            }

            return NoContent();
        }
        #endregion

        #region [ DELETE Methods ]
        [HttpDelete("{pointOfInterestId}", Name = "DeletePointOfInterestAsync")]
        public async Task<ActionResult> DeletePointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            var result = await _mediator.Send(new DeletePointOfInterestCommand(cityId, pointOfInterestId));

            if (result.PointOfInterestNotFound)
                return NotFound($"Point of Interest of city with cityId {cityId} was not found to delete!");

            return NoContent();
        }
        #endregion

        #region [ Validation ]
        public override ActionResult ValidationProblem(
        [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value
                .InvalidModelStateResponseFactory(ControllerContext);
        }
        #endregion
    }

}
