using AutoMapper;
using CityInfo.Application.DTOs;
using CityInfo.Domain.Entities;

namespace CityInfo.APIs.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        #region [ Constructure ]
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
            CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
            CreateMap<PointOfInterest, PointOfInterestForUpdateDto>();
        }
        #endregion
    }
}
