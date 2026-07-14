using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cargo_Management_Project.Models
{
    public class Container
    {
        [Key]
        public int ContainerId { get; set; }

        [Required]
        [StringLength(20)]
        public string ContainerNumber { get; set; } = string.Empty;

        [Required]
        public string ContainerType { get; set; } = "DRY"; // DRY, REEFER, FLAT_RACK, OPEN_TOP

        [Required]
        [StringLength(30)]
        public string SealNumber { get; set; } = string.Empty;

        [Required]
        public string ContainerStatus { get; set; } = "EMPTY"; // EMPTY, LOADED, IN_TRANSIT, DISCHARGED

        // Foreign Key definition targeting ShipmentBooking
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public ShipmentBooking? ShipmentBooking { get; set; }

        public virtual ICollection<CargoEvent> CargoEvents { get; set; } = new List<CargoEvent>();
    }
}
