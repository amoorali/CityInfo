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
    public class GetCityHandler : GeneralHandler,
        IRequestHandler<GetCityQuery, GetCityResult>
    {
        #region [ Constructor ]
        public GetCityHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMailService mailService,
            IPropertyCheckerService propertyCheckerService)
            : base(unitOfWork, mapper, mailService, propertyCheckerService)
        {
        }
        #endregion

        #region [ Handler ]
        public async Task<GetCityResult> Handle(
            GetCityQuery request,
            CancellationToken cancellationToken)
        {
            if (!PropertyCheckerService.TypeHasProperties<CityDto>(request.Fields))
                return new GetCityResult(true, null);

            var entity = await UnitOfWork.Cities
                .GetCityAsync(request.CityId, request.IncludePointsOfInterest);

            if (entity == null)
                return new GetCityResult(true, null);

            if (request.IncludePointsOfInterest)
            {
                var dtoWithPointsOfInterest = Mapper.Map<CityDto>(entity)
                    .ShapeData(request.Fields);
                return new GetCityResult(false, dtoWithPointsOfInterest);
            }

            var dtoWithoutPointsOfInterest = Mapper.Map<CityWithoutPointsOfInterestDto>(entity)
                .ShapeData(request.Fields);

            return new GetCityResult(false, dtoWithoutPointsOfInterest);
        }
        #endregion
    }
}
