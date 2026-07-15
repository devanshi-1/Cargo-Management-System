using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cargo_Management_Project.Data;
using Cargo_Management_Project.Models;
using Cargo_Management_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cargo_Management_Project.Controllers
{
    [Authorize]
    public class ShipmentBookingController : Controller
    {
        private readonly IShipmentBookingService _bookingService;

        // Dependency Injection uses Service instead of DbContext
        public ShipmentBookingController(IShipmentBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            var associatedName = User.FindFirst("AssociatedName")?.Value ?? string.Empty;

            var bookings = await _bookingService.GetBookingsAsync(role, associatedName);

            ViewBag.Role = role;
            ViewBag.AssociatedName = associatedName;
            return View(bookings);
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingDetailsAsync(id);
            if (booking == null) return NotFound();

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var associatedName = User.FindFirst("AssociatedName")?.Value;

            if (role == "Shipper" && booking.ShipperName != associatedName) return Forbid();
            if (role == "Consignee" && booking.ConsigneeName != associatedName) return Forbid();

            ViewBag.Role = role;
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipmentBooking booking)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            var associatedName = User.FindFirst("AssociatedName")?.Value ?? string.Empty;

            ModelState.Remove(nameof(booking.BookingNumber));
            ModelState.Remove(nameof(booking.BookingStatus));

            if (ModelState.IsValid)
            {
                var success = await _bookingService.CreateBookingAsync(booking, role, associatedName);
                if (success) return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to create shipment. Check fields.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Freight Forwarder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var success = await _bookingService.ConfirmBookingAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Booking confirmed and Bill of Lading Generated!";
            }
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Amend(ShipmentBooking booking)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            var associatedName = User.FindFirst("AssociatedName")?.Value ?? string.Empty;

            // Security check before calling service update
            var existing = await _bookingService.GetBookingDetailsAsync(booking.BookingId);
            if (existing == null) return NotFound();
            if (role == "Shipper" && existing.ShipperName != associatedName) return Forbid();

            ModelState.Remove(nameof(booking.BookingNumber));
            if (role == "Shipper") ModelState.Remove(nameof(booking.BookingStatus));

            if (ModelState.IsValid)
            {
                var success = await _bookingService.AmendBookingAsync(booking, role, associatedName);
                if (success)
                {
                    TempData["SuccessMessage"] = "Booking amended successfully.";
                    return RedirectToAction(nameof(Details), new { id = booking.BookingId });
                }
            }

            TempData["ErrorMessage"] = "Invalid input data or status restriction.";
            return RedirectToAction(nameof(Details), new { id = booking.BookingId });
        }
    }
}
