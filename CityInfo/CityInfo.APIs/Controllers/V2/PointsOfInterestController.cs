using Asp.Versioning;
using MapsterMapper;
using CityInfo.Application.DTOs;
using CityInfo.Infrastructure.Repositories.Contracts;
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
        private readonly ICityRepository _cityRepository;
        private readonly IPointOfInterestRepository _pointOfInterestRepository;
        private readonly IMapper _mapper;
        #endregion

        #region [ Constructure ]
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            ICityRepository cityRepository,
            IPointOfInterestRepository pointOfInterestRepository,
            IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
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
    }
}
