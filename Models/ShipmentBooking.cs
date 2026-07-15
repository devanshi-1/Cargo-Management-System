using System.ComponentModel.DataAnnotations;

namespace Cargo_Management_Project.Models
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
      
        // Navigation Properties for related records - initialized to prevent null-reference exceptions
        public ICollection<Container> Containers { get; set; } = new List<Container>();
        public ICollection<CustomsDeclaration> CustomsDeclarations { get; set; } = new List<CustomsDeclaration>();
        public ICollection<FreightInvoice> FreightInvoices { get; set; } = new List<FreightInvoice>();

    }
}
