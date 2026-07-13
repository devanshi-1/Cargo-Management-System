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

            booking.BookingNumber = "BKG-" + DateTime.Now.ToString("yyMMdd") + "-" + new Random().Next(1000, 9999);
            booking.BookingStatus = "DRAFT"; 

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

            if (booking.BookingStatus == "DRAFT")
            {
                booking.BookingStatus = "CONFIRMED"; 

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

            if (role == "Shipper" && existing.BookingStatus != "DRAFT")
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
    }
}