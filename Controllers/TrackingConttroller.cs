using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cargo_Management_Project.Data;
using Cargo_Management_Project.Services;
using Cargo_Management_Project.Models;

namespace Cargo_Management_Project.Controllers
{
    public class TrackingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ITrackingService _trackingService;

        public TrackingController(AppDbContext context, ITrackingService trackingService)
        {
            _context = context;
            _trackingService = trackingService;
        }

        [HttpPost]
        public async Task<IActionResult> RecordPortEvent(CargoEvent cargoEvent)
        {
            if (ModelState.IsValid)
            {
                bool result = await _trackingService.RecordPortEventAsync(cargoEvent);
                if (result)
                {
                    TempData["SuccessMessage"] = "Port event recorded successfully!";
                    return RedirectToAction("Index", "Container");
                }
                ModelState.AddModelError("", "Failed to record the port event. Please check the details.");
            }
            return View(cargoEvent);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLocation(int containerId, ContainerStatus newStatus)
        {
            var container = await _context.Containers.FindAsync(containerId);
            if (container == null)
            {
                return NotFound();
            }

            container.ContainerStatus = newStatus;

            _context.Containers.Update(container);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Container status updated successfully!";
            return RedirectToAction("Index", "Container");
        }

        // REQUIRED ACTION: Gets the tracking history of a cargo container
        [HttpGet]
        public async Task<IActionResult> GetCargoMilestones(int containerId)
        {
            var container = await _context.Containers
                .Include(c => c.CargoEvents)
                .FirstOrDefaultAsync(c => c.ContainerId == containerId);

            if (container == null)
            {
                return NotFound();
            }

            // Fixed: Orders by your model's actual property name 'EventTimestamp'
            var milestones = container.CargoEvents.OrderBy(e => e.EventTimestamp).ToList();
            return View(milestones);
        }

        // REQUIRED ACTION: Logs a delay event using correct model properties
        [HttpPost]
        public async Task<IActionResult> LogDelay(int containerId, string delayReason)
        {
            if (string.IsNullOrWhiteSpace(delayReason))
            {
                ModelState.AddModelError("delayReason", "Delay reason is required.");
                return RedirectToAction("GetCargoMilestones", new { containerId });
            }

            // Fixed: Maps to your exact CargoEvent.cs properties and enum type
            var delayEvent = new CargoEvent
            {
                ContainerId = containerId,
                EventType = EventType.GATE_IN, // Uses existing valid enum value
                EventLocation = "In Transit / Port",
                EventTimestamp = DateTime.UtcNow,
                Remarks = $"DELAY WARNING: {delayReason}"
            };

            bool result = await _trackingService.RecordPortEventAsync(delayEvent);
            if (result)
            {
                TempData["WarningMessage"] = "Delay logged successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to log the delay.";
            }

            return RedirectToAction("GetCargoMilestones", new { containerId });
        }
    }
}