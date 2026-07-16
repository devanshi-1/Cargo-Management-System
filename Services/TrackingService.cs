using System;
using System.Threading.Tasks;
using Cargo_Management_Project.Data;
using Cargo_Management_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargo_Management_Project.Services
{
    /// <summary>
    /// Implementation of tracking service for recording and managing cargo events.
    /// Handles port events, container status transitions, and shipment tracking records.
    /// </summary>
    public class TrackingService : ITrackingService
    {
        private readonly AppDbContext _context;

        public TrackingService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Records a port event (cargo event) in the tracking system.
        /// </summary>
        /// <param name="cargoEvent">The cargo event to record (e.g., container loading, port arrival, customs clearance)</param>
        /// <returns>True if the event was successfully recorded, false otherwise</returns>
        public async Task<bool> RecordPortEventAsync(CargoEvent cargoEvent)
        {
            try
            {
                if (cargoEvent == null)
                    throw new ArgumentNullException(nameof(cargoEvent), "Cargo event cannot be null.");

                // Validate that the container exists
                var container = await _context.Containers.FindAsync(cargoEvent.ContainerId);
                if (container == null)
                    throw new InvalidOperationException($"Container with ID {cargoEvent.ContainerId} not found.");

                // Set timestamp if not already set
                if (cargoEvent.EventTimestamp == default)
                    cargoEvent.EventTimestamp = DateTime.Now;

                // Add the event to the database
                _context.CargoEvents.Add(cargoEvent);
                var result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (DbUpdateException ex)
            {
                // Log database errors
                throw new InvalidOperationException("Failed to record cargo event due to database error.", ex);
            }
            catch (Exception ex)
            {
                // Log other unexpected errors
                throw new InvalidOperationException("An unexpected error occurred while recording the cargo event.", ex);
            }
        }
    }
}
