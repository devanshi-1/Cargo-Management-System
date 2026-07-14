using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoManagementSystem.Models
{
    public enum EventType { GATE_IN, LOADED, SAILED, DISCHARGED, GATE_OUT }
    public class CargoEvent
    {
        [Key]
        public int EventId { get; set; }

        [ForeignKey("Container")]
        public int ContainerId { get; set; }
        public Container Container { get; set; }

        public EventType EventType { get; set; }
        public string EventLocation { get; set; }
        public DateTime EventTimestamp { get; set; }
        public string Remarks { get; set; }
    }
}