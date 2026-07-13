using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cargo_Management_Project.Models
{
    public class ShipmentBooking
    {
        [Key] // Defines bookingId as the Primary Key
        public int BookingId { get; set; }

        [Required]
        [StringLength(30)]
        public string BookingNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ShipperName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ConsigneeName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string OriginPort { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string DestinationPort { get; set; } = string.Empty;

        [Required]
        public string BookingStatus { get; set; } = "DRAFT"; // DRAFT, CONFIRMED, CANCELLED, COMPLETED

        // Navigation Property: One Booking can have multiple Containers
        public ICollection<Container> Containers { get; set; } = new List<Container>();

        public CustomsDeclaration? CustomsDeclaration { get; set; }
        public FreightInvoice? FreightInvoice { get; set; }
    }
}
