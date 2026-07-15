
﻿using Cargo_Management_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cargo_Management_Project.Models
{
    public enum InvoiceStatus { DRAFT, ISSUED, PAID, OVERDUE }
    public class FreightInvoice
    {
        [Key]
        public int InvoiceId { get; set; }

        public int BookingId { get; set; }

        // Navigation property - single definition (avoid duplicates)
        [ForeignKey("BookingId")]
        public virtual ShipmentBooking? ShipmentBooking { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal FreightCharges { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal SurchargeAmount { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(10)]
        public string? Currency { get; set; }

        public InvoiceStatus InvoiceStatus { get; set; }
    }
}