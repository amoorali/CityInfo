using CityInfo.Application.Common.Contracts;
using CityInfo.Application.Common.Exceptions;
using CityInfo.Application.Common.Helpers;
using CityInfo.Application.DTOs.City;
using CityInfo.Application.Features.City.Queries;
using CityInfo.Application.Features.City.Results;
using CityInfo.Application.Repositories.Contracts;
using CityInfo.Application.Services.Contracts;
using Mapster;
using MediatR;
using System.Dynamic;
using System.Net.Http.Headers;

namespace CityInfo.Application.Features.City.Handlers
{
    public class GetCityHandler : IRequestHandler<GetCityQuery, GetCityResult>
    {
        #region [ Fields ]
        private readonly ICityRepository _cityRepository;
        private readonly IPropertyCheckerService _propertyCheckerService;
        private readonly ICityLinkService _cityLinkService;
        #endregion

        #region [ Constructor ]
        public GetCityHandler(
            ICityRepository cityRepository,
            IPropertyCheckerService propertyCheckerService,
            ICityLinkService cityLinkService)
        {
            _cityRepository = cityRepository;
            _propertyCheckerService = propertyCheckerService;
            _cityLinkService = cityLinkService;
        }
        #endregion

        #region [ Handler ]
        public async Task<GetCityResult> Handle(
            GetCityQuery request,
            CancellationToken cancellationToken)
        {
            if (!_propertyCheckerService.TypeHasProperties<CityDto>(request.Fields))
                throw new BadRequestException("One or more requested fields do not exist.");

            if (!MediaTypeHeaderValue.TryParse(request.MediaType, out var parsedMediaType))
                throw new BadRequestException("Accept header media type value is not a valid media type.");

            Domain.Entities.City? entity = request.IncludePointsOfInterest
                ? await _cityRepository.GetCityWithPointsOfInterestAsync(request.CityId).ConfigureAwait(false)
                : await _cityRepository.GetCityWithoutPointsOfInterestAsync(request.CityId).ConfigureAwait(false);

            if (entity == null)
                return new GetCityResult(NotFound: true, Item: null);

            ExpandoObject shapedDto = request.IncludePointsOfInterest
                ? entity.Adapt<CityDto>().ShapeData(request.Fields)
                : entity.Adapt<CityWithoutPointsOfInterestDto>().ShapeData(request.Fields);

            if (parsedMediaType.MediaType!.Equals("application/vnd.marvin.hateoas+json",
                StringComparison.OrdinalIgnoreCase))
            {
                var linkedResources = (IDictionary<string, object?>)shapedDto;

                var links = _cityLinkService.CreateLinksForCity(request.CityId, request.Fields);

                linkedResources.Add("links", links);

                return new GetCityResult(NotFound: false, Item: linkedResources);
            }

            return new GetCityResult(NotFound: false, Item: shapedDto);
        }
        #endregion
    }
}
