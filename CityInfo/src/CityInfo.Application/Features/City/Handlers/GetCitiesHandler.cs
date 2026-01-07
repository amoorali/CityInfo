using CityInfo.Application.Common;
using CityInfo.Application.Common.Exceptions;
using CityInfo.Application.Common.Helpers;
using CityInfo.Application.DTOs.City;
using CityInfo.Application.Features.City.Queries;
using CityInfo.Application.Features.City.Results;
using CityInfo.Application.Repositories.Contracts;
using CityInfo.Application.Services.Contracts;
using Mapster;
using MediatR;

namespace CityInfo.Application.Features.City.Handlers
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesQuery, GetCitiesResult>
    {
        #region [ Fields ]
        private readonly ICityRepository _cityRepository;
        private readonly IPropertyCheckerService _propertyCheckerService;
        private readonly IPropertyMappingService _propertyMappingService;
        #endregion

        #region [ Constructor ]
        public GetCitiesHandler(
            ICityRepository cityRepository,
            IPropertyCheckerService propertyCheckerService,
            IPropertyMappingService propertyMappingService)
        {
            _cityRepository = cityRepository;
            _propertyCheckerService = propertyCheckerService;
            _propertyMappingService = propertyMappingService;
        }
        #endregion

        #region [ Handler ]
        public async Task<GetCitiesResult> Handle(
            GetCitiesQuery request,
            CancellationToken cancellationToken)
        {
            var parameters = request.CitiesResourceParameters;

            if (!_propertyMappingService.
                ValidMappingExistsFor<CityDto, Domain.Entities.City>(parameters.OrderBy))
            {
                throw new BadRequestException("Invalid sorting fields.");
            }

            if (!_propertyCheckerService.TypeHasProperties<CityWithoutPointsOfInterestDto>
                (parameters.Fields))
            {
                throw new BadRequestException("No cities found");
            }

            var query = _cityRepository.QueryCities();

            #region [ Filter ]
            var name = parameters.Name?.Trim();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name == name);
            #endregion

            #region [ Search Query ]
            var searchQuery = parameters.SearchQuery?.Trim();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(c =>
                    c.Name.Contains(searchQuery) ||
                    !string.IsNullOrEmpty(c.Description) &&
                    c.Description.Contains(searchQuery));
            }
            #endregion

            #region [ Sorting ]
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy) &&
                parameters.OrderBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderBy(c => c.Name);
            }
            #endregion

            var pagedEntities = await PagedList<Domain.Entities.City>.CreateAsync(
                query,
                parameters.PageNumber,
                parameters.PageSize);

            var paginationMetadata = new PaginationMetadata(
                pagedEntities.TotalCount,
                pagedEntities.PageSize,
                pagedEntities.CurrentPage,
                pagedEntities.TotalPages);

            var dtos = pagedEntities.Items
                .Adapt<IEnumerable<CityWithoutPointsOfInterestDto>>()
                .ShapeData(parameters.Fields);

            return new GetCitiesResult(
                dtos,
                pagedEntities.HasPrevious,
                pagedEntities.HasNext,
                paginationMetadata);
        }
        #endregion
    }
}
