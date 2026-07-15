<<<<<<< HEAD
﻿using Cargo_Management_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cargo_Management_Project.Models
=======
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoManagementSystem.Models
>>>>>>> 550e0d1bfe23c637ed19a35e74cd66e93e8ab2e4
{
    public enum InvoiceStatus { DRAFT, ISSUED, PAID, OVERDUE }
    public class FreightInvoice
    {
        [Key]
        public int InvoiceId { get; set; }

<<<<<<< HEAD
        public int BookingId { get; set; }
=======
        [ForeignKey("ShipmentBooking")]
        public int BookingId { get; set; }
        public ShipmentBooking ShipmentBooking { get; set; }
>>>>>>> 550e0d1bfe23c637ed19a35e74cd66e93e8ab2e4

        [Column(TypeName = "decimal(15,2)")]
        public decimal FreightCharges { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal SurchargeAmount { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal TotalAmount { get; set; }

<<<<<<< HEAD
        [StringLength(10)]
        public string? Currency { get; set; }

        [StringLength(20)]
        public string? InvoiceStatus { get; set; }

        [ForeignKey("BookingId")]
        public virtual ShipmentBooking? ShipmentBooking { get; set; }
=======
        public string Currency { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
>>>>>>> 550e0d1bfe23c637ed19a35e74cd66e93e8ab2e4
    }
}