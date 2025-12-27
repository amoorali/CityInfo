using System.ComponentModel.DataAnnotations;

namespace CityInfo.Application.DTOs.PointOfInterest
{
    public class PointOfInterestForUpdateDto : PointOfInterestForManipulationDto
    {
        #region [ Fields ]
        [Required(ErrorMessage = "You should fill out a description.")]
        public override string? Description { 
            get => base.Description; set => base.Description = value; }
        #endregion
    }
}
