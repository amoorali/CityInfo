using CityInfo.Application.DTOs.PointOfInterest;

namespace CityInfo.Application.DTOs.City
{
    public class CityDto
    {
        #region [ Fields ]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NumberOfPointsOfInterest
        {
            get { return PointsOfInterest.Count; }
        }

        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; }
            = new List<PointOfInterestDto>();
        #endregion
    }
}
