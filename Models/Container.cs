using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoManagementSystem.Models
{
    public enum ContainerType { DRY, REEFER, FLAT_RACK, OPEN_TOP }
    public enum ContainerStatus { EMPTY, LOADED, IN_TRANSIT, DISCHARGED }
    public class Container
    {
        [Key]
        public int ContainerId { get; set; }
        public string ContainerNumber { get; set; }
        public ContainerType ContainerType { get; set; }

        [ForeignKey("ShipmentBooking")]
        public int BookingId { get; set; }
        public ShipmentBooking ShipmentBooking { get; set; }

        public string SealNumber { get; set; }
        public ContainerStatus ContainerStatus { get; set; }

        // Navigation Property
        public ICollection<CargoEvent> CargoEvents { get; set; }
    }
}