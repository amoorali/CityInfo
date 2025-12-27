using Mapster;
using CityInfo.Domain.Entities;
using CityInfo.Application.DTOs.PointOfInterest;

namespace CityInfo.Application.Mapping
{
    public class PointOfInterestMapping : IRegister
    {
        #region [ Mapster Configuration ]
        public void Register(TypeAdapterConfig config)
        {
            #region [ Map PointOfInterest to PointOfInterestDto ]
            config.NewConfig<PointOfInterest, PointOfInterestDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description);
            #endregion

            #region [ Map PointOfInterestForCreationDto to PointOfInterest ]
            config.NewConfig<PointOfInterestForCreationDto, PointOfInterest>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description);
            #endregion

            #region [ Map PointOfInterestForUpdateDto to PointOfInterest (TwoWays) ]
            config.NewConfig<PointOfInterest, PointOfInterestForUpdateDto>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .TwoWays();
            #endregion
        }
        #endregion
    }
}
