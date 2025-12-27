using System.ComponentModel.DataAnnotations;

namespace CityInfo.Application.DTOs.PointOfInterest
{
    public abstract class PointOfInterestForManipulationDto
    {
        #region [ Fields ]
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(100, ErrorMessage = "The name shouldn't have more than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1500, ErrorMessage = "The description shouldn't have more than 1500 characters.")]
        public virtual string? Description { get; set; }
        #endregion
    }
}
