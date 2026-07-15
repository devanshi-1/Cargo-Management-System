using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cargo_Management_Project.Models
{
    public enum EventType { GATE_IN, LOADED, SAILED, DISCHARGED, GATE_OUT }
    public class CargoEvent
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [ForeignKey("Container")]
        public int ContainerId { get; set; }
        public Container? Container { get; set; }

        public EventType EventType { get; set; }
        public string? EventLocation { get; set; }
        public DateTime EventTimestamp { get; set; }
        public string? Remarks { get; set; }

        public Container Container { get; set; }
      
        [Required]
        public EventType EventType { get; set; }

        [Required]
        [StringLength(100)]
        public string EventLocation { get; set; } = string.Empty;

        [Required]
        public DateTime EventTimestamp { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string Remarks { get; set; } = string.Empty;

    }
}