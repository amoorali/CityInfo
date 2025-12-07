namespace CityInfo.Application.DTOs
{
    public class PointOfInterestDto
    {
        #region [ Fields ]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        #endregion
    }
}
