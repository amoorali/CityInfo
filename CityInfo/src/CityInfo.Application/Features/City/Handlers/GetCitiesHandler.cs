using CityInfo.Application.Common;
using CityInfo.Application.Common.Helpers;
using CityInfo.Application.Common.ResourceParameters;
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
        public GetCitiesHandler(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
            : base(unitOfWork, mapper, mailService)
        {
        }

        public async Task<GetCitiesResult> Handle(
            GetCitiesQuery request,
            CancellationToken cancellationToken)
        {
            var pagedEntities = await UnitOfWork.Cities
                .GetCitiesAsync(request.CitiesResourceParameters);

            var paginationMetadata = new PaginationMetadata(
                pagedEntities.TotalCount,
                pagedEntities.PageSize,
                pagedEntities.CurrentPage,
                pagedEntities.TotalPages);

            var dtos = Mapper.Map<IReadOnlyList<CityWithoutPointsOfInterestDto>>(pagedEntities.Items);

            return new GetCitiesResult(
                dtos,
                pagedEntities.HasPrevious,
                pagedEntities.HasNext,
                paginationMetadata);
        }
    }
}
