using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.Domain.Entities
{
    public class City
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

        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
            = new List<PointOfInterest>();
        #endregion

        #region [ Constructure ]
        public City(string name)
        {
            Name = name;
        }
        #endregion
    }
}
