using CityInfo.Application.Common;
using CityInfo.Application.Common.Helpers;
using CityInfo.Application.DTOs.City;
using CityInfo.Application.Features.BaseImplementations;
using CityInfo.Application.Features.City.Queries;
using CityInfo.Application.Features.City.Results;
using CityInfo.Application.Services.Contracts;
using MapsterMapper;
using MediatR;

namespace CityInfo.Application.Features.City.Handlers
{
    public class GetCitiesHandler : GeneralHandler,
        IRequestHandler<GetCitiesQuery, GetCitiesResult>
    {
        #region [ Constructor ]
        public GetCitiesHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMailService mailService,
            IPropertyCheckerService propertyCheckerService)
            : base(unitOfWork, mapper, mailService, propertyCheckerService)
        {
        }
        #endregion

        #region [ Handler ]
        public async Task<GetCitiesResult> Handle(
            GetCitiesQuery request,
            CancellationToken cancellationToken)
        {
            if (!PropertyCheckerService.TypeHasProperties<CityWithoutPointsOfInterestDto>
                (request.CitiesResourceParameters.Fields))
            {
                return new GetCitiesResult(null, false, false, null);
            }

            var pagedEntities = await UnitOfWork.Cities
                .GetCitiesAsync(request.CitiesResourceParameters);

            var paginationMetadata = new PaginationMetadata(
                pagedEntities.TotalCount,
                pagedEntities.PageSize,
                pagedEntities.CurrentPage,
                pagedEntities.TotalPages);

            var dtos = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(pagedEntities.Items)
                             .ShapeData(request.CitiesResourceParameters.Fields);

            return new GetCitiesResult(
                dtos,
                pagedEntities.HasPrevious,
                pagedEntities.HasNext,
                paginationMetadata);
        }
        #endregion
    }
}
