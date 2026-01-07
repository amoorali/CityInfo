using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Repositories.Contracts;
using CityInfo.Application.Services.Contracts;
using Mapster;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class UpdatePointOfInterestHandler : IRequestHandler<UpdatePointOfInterestCommand, UpdatePointOfInterestResult>
    {
        #region [ Fields ]
        private readonly ICityRepository _cityRepository;
        private readonly IPointOfInterestRepository _pointOfInterestRepository;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region [ Constructor ]
        public UpdatePointOfInterestHandler(
            ICityRepository cityRepository,
            IPointOfInterestRepository pointOfInterestRepository,
            IUnitOfWork unitOfWork)
        {
            _cityRepository = cityRepository;
            _pointOfInterestRepository = pointOfInterestRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region [ Handler ]
        public async Task<UpdatePointOfInterestResult> Handle(
            UpdatePointOfInterestCommand request,
            CancellationToken cancellationToken)
        {
            if (!await _cityRepository.CityExistsAsync(request.CityId))
                return new UpdatePointOfInterestResult(true, true);

            var entity = await _pointOfInterestRepository
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (entity == null)
                return new UpdatePointOfInterestResult(false, true);

            request.Dto.Adapt(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdatePointOfInterestResult(false, false);
        }
        #endregion
    }
}
