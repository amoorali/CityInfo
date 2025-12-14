using Mapster;
using CityInfo.Application.DTOs;
using CityInfo.Domain.Entities;

namespace CityInfo.Application.Mapping
{
    public class CityMapping : IRegister
    {
        #region [ Mapster Configuration ]
        public void Register(TypeAdapterConfig config)
        {
            #region [ Map City to CityDto ]
            config.NewConfig<City, CityDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.PointsOfInterest, src => src.PointsOfInterest);
            #endregion

            #region [ Map City to CityWithoutPointsOfInterestDto ]
            config.NewConfig<City, CityWithoutPointsOfInterestDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description);
            #endregion
        }
        #endregion
    }
}
