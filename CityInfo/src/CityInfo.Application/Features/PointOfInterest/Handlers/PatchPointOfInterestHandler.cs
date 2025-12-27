using CityInfo.Application.DTOs.PointOfInterest;
using CityInfo.Application.Features.BaseImplementations;
using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Services.Contracts;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class PatchPointOfInterestHandler : GeneralHandler,
        IRequestHandler<PatchPointOfInterestCommand, PatchPointOfInterestResult>
    {
        private readonly IValidator<PointOfInterestForUpdateDto> _validator;

        public PatchPointOfInterestHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMailService mailService,
            IValidator<PointOfInterestForUpdateDto> validator)
            : base(unitOfWork, mapper, mailService)
        {
            _validator = validator;
        }

        public async Task<PatchPointOfInterestResult> Handle(
            PatchPointOfInterestCommand request,
            CancellationToken cancellationToken)
        {
            if (!await UnitOfWork.Cities.CityExistsAsync(request.CityId))
                return new PatchPointOfInterestResult(true, true, null, null);

            var entity = await UnitOfWork.PointsOfInterest
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (entity == null)
                return new PatchPointOfInterestResult(false, true, null, null);

            var dtoToPatch = Mapper.Map<PointOfInterestForUpdateDto>(entity);

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

            Mapper.Map(dtoToPatch, entity);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return new PatchPointOfInterestResult(false, false, dtoToPatch, null);
        }
    }
}
