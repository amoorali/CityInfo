namespace CityInfo.Application.DTOs.City
{
    public abstract class CityForManipulationDto
    {
        #region [ Fields ]
        public string Name { get; set; } = string.Empty;

        public virtual string? Description { get; set; }
        #endregion
    }
}
