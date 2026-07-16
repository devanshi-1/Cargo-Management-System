using System.Collections.Generic;
using System.Threading.Tasks;
using Cargo_Management_Project.Models;

namespace Cargo_Management_Project.Services
{
    public interface IShipmentBookingService
    {
        Task<IEnumerable<ShipmentBooking>> GetBookingsAsync(string role, string associatedName);
        Task<ShipmentBooking?> GetBookingDetailsAsync(int id);
        Task<bool> CreateBookingAsync(ShipmentBooking booking, string role, string associatedName);
        Task<bool> ConfirmBookingAsync(int id);
        Task<bool> AmendBookingAsync(ShipmentBooking booking, string role, string associatedName);
    }
}