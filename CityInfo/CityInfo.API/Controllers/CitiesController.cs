using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Interfaces;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepostory;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepostory,
            IMapper mapper)
        {
            _cityInfoRepostory = cityInfoRepostory ??
                throw new ArgumentNullException(nameof(cityInfoRepostory));
            _mapper = mapper 
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCitiesAsync(
            string? name, string? searchQuery)
        {
            var cityEntities = await _cityInfoRepostory.GetCitiesAsync(name, searchQuery);

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityAsync(
            int id, bool includePointsOfInterest = false)
        {
            var cityEntity = await _cityInfoRepostory.GetCityAsync(id, includePointsOfInterest);

            if (cityEntity == null)
                return NotFound("The id isn't in the collection");

            if (includePointsOfInterest)
                return Ok(_mapper.Map<CityDto>(cityEntity));

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(cityEntity));
        }
    }
}
