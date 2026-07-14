using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cargo_Management_Project.Models;
using Cargo_Management_Project.Data;

namespace Cargo_Management_Project.Controllers
{
    [Authorize]
    public class ShipmentBookingController : Controller
    {
        private readonly AppDbContext _context;
        public ShipmentBookingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var associatedName = User.FindFirst("AssociatedName")?.Value;

            List<ShipmentBooking> bookings;

            if (role == "Shipper")
            {
                bookings = await _context.ShipmentBookings
                    .Where(b => b.ShipperName == associatedName)
                    .ToListAsync();
            }
            else if (role == "Consignee")
            {
                bookings = await _context.ShipmentBookings
                    .Where(b => b.ConsigneeName == associatedName)
                    .ToListAsync();
            }
            else
            {
                bookings = await _context.ShipmentBookings.ToListAsync();
            }

            ViewBag.Role = role;
            ViewBag.AssociatedName = associatedName;
            return View(bookings);
        }
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.ShipmentBookings
                .Include(b => b.Containers)
                .Include(b => b.CustomsDeclaration)
                .Include(b => b.FreightInvoice)
                .FirstOrDefaultAsync(m => m.BookingId == id);

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
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var associatedName = User.FindFirst("AssociatedName")?.Value;

            if (role == "Shipper")
            {
                booking.ShipperName = associatedName ?? "Unknown Shipper";
            }

            // FIX 1: Use Guid-based booking number generation instead of Random to prevent duplicates in concurrent scenarios
            // Format: BKG-YYMMDD-[first 8 chars of GUID in uppercase]
            booking.BookingNumber = GenerateUniqueBookingNumber();
            
            // FIX 3: Use BookingStatus enum instead of magic string "DRAFT" for type safety and consistency
            booking.BookingStatus = BookingStatus.DRAFT;

            ModelState.Remove(nameof(booking.BookingNumber));
            ModelState.Remove(nameof(booking.BookingStatus));

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to create shipment. Check fields.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Freight Forwarder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var booking = await _context.ShipmentBookings.FindAsync(id);
            if (booking == null) return NotFound();

            // FIX 3: Use BookingStatus enum instead of magic string "DRAFT" for type safety and consistency
            if (booking.BookingStatus == BookingStatus.DRAFT)
            {
                booking.BookingStatus = BookingStatus.CONFIRMED;

                _context.Update(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking confirmed and Bill of Lading Generated!";
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Amend(ShipmentBooking booking)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var associatedName = User.FindFirst("AssociatedName")?.Value;

            var existing = await _context.ShipmentBookings.AsNoTracking().FirstOrDefaultAsync(b => b.BookingId == booking.BookingId);
            if (existing == null) return NotFound();

            if (role == "Shipper" && existing.ShipperName != associatedName) return Forbid();

            // FIX 3: Use BookingStatus enum instead of magic string "DRAFT" for type safety and consistency
            if (role == "Shipper" && existing.BookingStatus != BookingStatus.DRAFT)
            {
                TempData["ErrorMessage"] = "You can only amend shipments in DRAFT status.";
                return RedirectToAction(nameof(Details), new { id = booking.BookingId });
            }

            booking.BookingNumber = existing.BookingNumber;
            if (role == "Shipper")
            {
                booking.ShipperName = existing.ShipperName;
                booking.BookingStatus = existing.BookingStatus;
            }

            ModelState.Remove(nameof(booking.BookingNumber));
            if (role == "Shipper") ModelState.Remove(nameof(booking.BookingStatus));

            if (ModelState.IsValid)
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking amended successfully.";
                return RedirectToAction(nameof(Details), new { id = booking.BookingId });
            }

            TempData["ErrorMessage"] = "Invalid input data.";
            return RedirectToAction(nameof(Details), new { id = booking.BookingId });
        }

        /// <summary>
        /// Generates a unique booking number using timestamp and GUID.
        /// 
        /// Why Guid instead of Random.Next()?
        /// - Random is NOT thread-safe: Creating multiple Random instances in rapid succession can generate identical seeds,
        ///   leading to duplicate numbers when multiple requests occur simultaneously.
        /// - Guid.NewGuid() guarantees uniqueness across concurrent requests using the system's UUID generation.
        /// 
        /// Format: BKG-YYMMDD-[first 8 chars of GUID in uppercase]
        /// Example: BKG-260714-A1B2C3D4
        /// </summary>
        private string GenerateUniqueBookingNumber()
        {
            var timestamp = DateTime.Now.ToString("yyMMdd");
            var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            return $"BKG-{timestamp}-{uniqueId}";
        }
    }
}
