using System.ComponentModel.DataAnnotations;

namespace CargoManagementSystem.Models
{
    public enum BookingStatus { DRAFT, CONFIRMED, CANCELLED, COMPLETED }

    public class ShipmentBooking
    {        
        [Key]
        public int BookingId { get; set; }
        public string BookingNumber { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string OriginPort { get; set; }
        public string DestinationPort { get; set; }
        public BookingStatus BookingStatus { get; set; }

        // Navigation Properties for related records
        public ICollection<Container> Containers { get; set; }
        public ICollection<CustomsDeclaration> CustomsDeclarations { get; set; }
        public ICollection<FreightInvoice> FreightInvoices { get; set; }
    }
}