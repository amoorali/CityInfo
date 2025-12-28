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
            var (entities, paginationMetadata) = await UnitOfWork.Cities
                .GetCitiesAsync(request.CitiesResourceParameters.Name, request.CitiesResourceParameters.SearchQuery,
                    request.PageNumber, request.PageSize);

            var dtos = Mapper.Map<IReadOnlyList<CityWithoutPointsOfInterestDto>>(entities);

            return new GetCitiesResult(dtos, paginationMetadata);
        }
    }
}
