using AutoMapper;
using CityInfo.Application.DTOs;
using CityInfo.Domain.Entities;

namespace CityInfo.APIs.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointsOfInterestDto>();
            CreateMap<City, CityDto>();
        }
    }
}
