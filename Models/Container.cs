using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cargo_Management_Project.Models
{
    public class Container
    {
        [Key]
        public int ContainerId { get; set; }

        [Required]
        [StringLength(20)]
        public string ContainerNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string ContainerType { get; set; } // DRY, REEFER, FLAT_RACK, OPEN_TOP

        public int BookingId { get; set; }

        [StringLength(30)]
        public string SealNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string ContainerStatus { get; set; } // EMPTY, LOADED, IN_TRANSIT, DISCHARGED
    }
}