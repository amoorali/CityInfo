using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.Domain.Entities
{
    public class PointOfInterest
    {
        #region [ Fields ]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }


        [ForeignKey("CityId")]
        public City? City { get; set; }
        public int CityId { get; set; }
        #endregion

        #region [ Constructure ]
        public PointOfInterest(string name)
        {
            Name = name;
        }
        #endregion
    }
}
