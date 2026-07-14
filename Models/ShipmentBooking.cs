using System.ComponentModel.DataAnnotations;

namespace CargoManagementSystem.Models
{
    public enum BookingStatus { DRAFT, CONFIRMED, CANCELLED, COMPLETED }

    public class ShipmentBooking
    {
        [Key]
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
        public BookingStatus BookingStatus { get; set; }

        // Navigation Properties for related records
        public ICollection<Container> Containers { get; set; }
        public ICollection<CustomsDeclaration> CustomsDeclarations { get; set; }
        public ICollection<FreightInvoice> FreightInvoices { get; set; }

    }
}