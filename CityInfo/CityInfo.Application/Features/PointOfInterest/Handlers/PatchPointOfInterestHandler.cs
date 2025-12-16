using CityInfo.Application.DTOs;
using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Services.Contracts;
using MapsterMapper;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class PatchPointOfInterestHandler : GeneralHandler,
        IRequestHandler<PatchPointOfInterestCommand, PatchPointOfInterestResult>
    {
        public PatchPointOfInterestHandler(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
            : base(unitOfWork, mapper, mailService)
        {
        }

        public async Task<PatchPointOfInterestResult> Handle(
            PatchPointOfInterestCommand request,
            CancellationToken cancellationToken)
        {
            if (!await UnitOfWork.Cities.CityExistsAsync(request.CityId))
                return new PatchPointOfInterestResult(false, false, null, null);

            var entity = await UnitOfWork.PointsOfInterest
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (entity == null)
                return new PatchPointOfInterestResult(true, false, null, null);

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

            Mapper.Map(dtoToPatch, entity);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return new PatchPointOfInterestResult(true, true, dtoToPatch, null);
        }
    }
}
