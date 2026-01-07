using CityInfo.Application.DTOs.PointOfInterest;
using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Repositories.Contracts;
using CityInfo.Application.Services.Contracts;
using FluentValidation;
using Mapster;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class PatchPointOfInterestHandler : IRequestHandler<PatchPointOfInterestCommand, PatchPointOfInterestResult>
    {
        #region [ Fields ]
        private readonly ICityRepository _cityRepository;
        private readonly IPointOfInterestRepository _pointOfInterestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<PointOfInterestForUpdateDto> _validator;
        #endregion

        #region [ Constructor ]
        public PatchPointOfInterestHandler(
            ICityRepository cityRepository,
            IPointOfInterestRepository pointOfInterestRepository,
            IUnitOfWork unitOfWork,
            IValidator<PointOfInterestForUpdateDto> validator)
        {
            _cityRepository = cityRepository;
            _pointOfInterestRepository = pointOfInterestRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        #endregion

        #region [ Handler ]
        public async Task<PatchPointOfInterestResult> Handle(
            PatchPointOfInterestCommand request,
            CancellationToken cancellationToken)
        {
            if (!await _cityRepository.CityExistsAsync(request.CityId))
                return new PatchPointOfInterestResult(true, true, null, null);

            var entity = await _pointOfInterestRepository
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (entity == null)
                return new PatchPointOfInterestResult(false, true, null, null);

            var dtoToPatch = entity
                .Adapt<PointOfInterestForUpdateDto>();

            var patchErrors = new Dictionary<string, string>();

            request.PatchDocument.ApplyTo(dtoToPatch, error =>
            {
                var key = string.IsNullOrWhiteSpace(error.AffectedObject?.ToString())
                    ? error.Operation.path ?? ""
                    : error.Operation.path ?? "";

                patchErrors[key] = error.ErrorMessage;
            });

            if (patchErrors.Count > 0)
                return new PatchPointOfInterestResult(false, false, null, patchErrors);

            var validation = await _validator.ValidateAsync(dtoToPatch, cancellationToken);
            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => string.Join(" | ", g.Select(e => e.ErrorMessage)));

                return new PatchPointOfInterestResult(false, false, null, errors);
            }
            
            dtoToPatch.Adapt(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PatchPointOfInterestResult(false, false, dtoToPatch, null);
        }
        #endregion
    }
}
