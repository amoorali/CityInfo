using Asp.Versioning;
using AutoMapper;
using CityInfo.Application.DTOs;
using CityInfo.Application.Services.Contracts;
using CityInfo.Infrastructure.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        private readonly IMailService _mailService;
        private readonly ICityRepository _cityRepository;
        private readonly IPointOfInterestRepository _pointOfInterestRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [ Constructure ]
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityRepository cityRepository,
            IPointOfInterestRepository pointOfInterestRepository,
            IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ??
                throw new ArgumentNullException(nameof(mailService));
            _cityRepository = cityRepository ??
                throw new ArgumentNullException(nameof(cityRepository));
            _pointOfInterestRepository = pointOfInterestRepository ??
                throw new ArgumentNullException(nameof(pointOfInterestRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        #endregion

        #region [ GET Methods ]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterestAsync(int cityId)
        {
            var cityName = User.Claims.First(c => c.Type == "city").Value;

            if (!await _cityRepository.CityNameMatchesCityIdAsync(cityName, cityId))
                return Forbid();

            if (!await _cityRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");

                return NotFound("City not found");
            }

            var pointsOfInterest = await _pointOfInterestRepository
                .GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest));
            
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            if (!await _cityRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");

                return NotFound("City not found");
            }

            var pointOfInterest = await _pointOfInterestRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
                return NotFound("City not found");

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }
        #endregion

        #region [ POST Methods ]
        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _cityRepository.CityExistsAsync(cityId))
                return NotFound("City not found!");

            var finalPointOfInterest = _mapper.Map<Domain.Entities.PointOfInterest>(pointOfInterest);

            await _pointOfInterestRepository.AddPointOfInterestForCityAsync(
                cityId, finalPointOfInterest);

            await _cityRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn =
                _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId,
                    pointOfInterestId = createdPointOfInterestToReturn.Id
                },
                createdPointOfInterestToReturn);
        }
        #endregion

        #region [ PUT Methods ]
        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
            if(!await _cityRepository.CityExistsAsync(cityId))
                return NotFound("City not found!");

            var pointOfInterestEntity = await _pointOfInterestRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
                return NotFound($"Point of Interest of city with cityId {cityId} was not found to create!");

            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            await _cityRepository.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region [ PATCH Methods ]
        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityRepository.CityExistsAsync(cityId))
                return NotFound("City not found!");

            var pointOfInterestEntity = await _pointOfInterestRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
                return NotFound($"Point of Interest of city with cityId {cityId} was not found to update!");

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(pointOfInterestToPatch))
                return BadRequest(ModelState);

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            await _cityRepository.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region [ DELETE Methods ]
        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            if (!await _cityRepository.CityExistsAsync(cityId))
                return NotFound("City not found!");

            var pointOfInterestEntity = await _pointOfInterestRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
                return NotFound($"Point of Interest of city with cityId {cityId} was not found to delete!");

            _pointOfInterestRepository.DeletePointOfInterest(pointOfInterestEntity);

            await _cityRepository.SaveChangesAsync();

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            return NoContent();
        }
        #endregion
    }
}
