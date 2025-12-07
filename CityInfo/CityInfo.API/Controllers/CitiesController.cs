using Asp.Versioning;
using AutoMapper;
using CityInfo.Domain.Entities;
using CityInfo.Application.DTOs;
using CityInfo.Infrastructure.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityInfo.APIs.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/v{version:apiVersion}/cities")]
    [ApiVersion(1)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepostory;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

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
            string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
                pageSize = maxCitiesPageSize;

            var (cityEntities, paginationMetadata) = await _cityInfoRepostory
                .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }

        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="id">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
        /// <returns>A city with or without points of interest</returns>
        /// <response code="200">Returns the requested city</response>
        /// <response code="404"></response>
        /// <response code="400"></response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
