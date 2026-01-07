using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Repositories.Contracts;
using CityInfo.Application.Services.Contracts;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class DeletePointOfInterestHandler : IRequestHandler<DeletePointOfInterestCommand, DeletePointOfInterestResult>
    {
        #region [ Fields ]
        private readonly ICityRepository _cityRepository;
        private readonly IPointOfInterestRepository _pointOfInterestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        #endregion

        #region [ Constructor ]
        public DeletePointOfInterestHandler(
            ICityRepository cityRepository,
            IPointOfInterestRepository pointOfInterestRepository,
            IUnitOfWork unitOfWork,
            IMailService mailService)
        {
            _cityRepository = cityRepository;
            _pointOfInterestRepository = pointOfInterestRepository;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }
        #endregion

        #region [ Handler ]
        public async Task<DeletePointOfInterestResult> Handle(
            DeletePointOfInterestCommand request,
            CancellationToken cancellationToken)
        {
            if (!await _cityRepository.CityExistsAsync(request.CityId))
                return new DeletePointOfInterestResult(true, true);

            var entity = await _pointOfInterestRepository
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (entity == null)
                return new DeletePointOfInterestResult(false, true);

            _pointOfInterestRepository.Remove(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {entity.Name} with id {entity.Id} was deleted.");

            return new DeletePointOfInterestResult(false, false);
        }
        #endregion
    }
}
