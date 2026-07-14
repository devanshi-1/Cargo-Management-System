using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoManagementSystem.Models
{
    public enum ClearanceStatus { FILED, UNDER_REVIEW, CLEARED, HELD, REJECTED }
    public class CustomsDeclaration
    {
        [Key]
        public int DeclarationId { get; set; }

        [ForeignKey("ShipmentBooking")]
        public int BookingId { get; set; }
        public ShipmentBooking ShipmentBooking { get; set; }

        public string HsCode { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal DeclaredValue { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal CalculatedDuty { get; set; }

        public ClearanceStatus ClearanceStatus { get; set; }
    }
}