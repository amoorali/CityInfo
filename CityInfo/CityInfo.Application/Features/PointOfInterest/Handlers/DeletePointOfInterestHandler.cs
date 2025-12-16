using CityInfo.Application.Features.BaseImplementations;
using CityInfo.Application.Features.PointOfInterest.Commands;
using CityInfo.Application.Features.PointOfInterest.Results;
using CityInfo.Application.Services.Contracts;
using MapsterMapper;
using MediatR;

namespace CityInfo.Application.Features.PointOfInterest.Handlers
{
    public class DeletePointOfInterestHandler : GeneralHandler,
        IRequestHandler<DeletePointOfInterestCommand, DeletePointOfInterestResult>
    {
        public DeletePointOfInterestHandler(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
            : base(unitOfWork, mapper, mailService)
        {
        }

        public async Task<DeletePointOfInterestResult> Handle(
            DeletePointOfInterestCommand request,
            CancellationToken cancellationToken)
        {
            if (!await UnitOfWork.Cities.CityExistsAsync(request.CityId))
                return new DeletePointOfInterestResult(true, true);

            var entity = await UnitOfWork.PointsOfInterest
                .GetPointOfInterestForCityAsync(request.CityId, request.PointOfInterestId);

            if (entity == null)
                return new DeletePointOfInterestResult(false, true);

            UnitOfWork.PointsOfInterest.DeletePointOfInterest(entity);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            MailService.Send("Point of interest deleted.",
                $"Point of interest {entity.Name} with id {entity.Id} was deleted.");

            return new DeletePointOfInterestResult(false, false);
        }
    }
}
