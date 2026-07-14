using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; 

namespace Cargo_Management_Project.Models
{
    public class CargoEvent
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public int ContainerId { get; set; }

        [ForeignKey("ContainerId")]
        public virtual Container? Container { get; set; }

        [Required]
        public string EventType { get; set; } = "GATE_IN"; 

        [Required]
        [StringLength(100)]
        public string EventLocation { get; set; } = string.Empty;

        [Required]
        public DateTime EventTimestamp { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string Remarks { get; set; } = string.Empty;
    }
}