using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cargo_Management_Project.Models
{
    public enum InvoiceStatus { DRAFT, ISSUED, PAID, OVERDUE }
    public class FreightInvoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [ForeignKey("ShipmentBooking")]
        public int BookingId { get; set; }
        public ShipmentBooking ShipmentBooking { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal FreightCharges { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal SurchargeAmount { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal TotalAmount { get; set; }

        public string Currency { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
    }
}
