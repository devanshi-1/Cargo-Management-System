using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cargo_Management_Project.Data;
using Cargo_Management_Project.Models;
using Cargo_Management_Project.Helpers;

namespace Cargo_Management_Project.Services
{
    public class ShipmentBookingService : IShipmentBookingService
    {
        private readonly AppDbContext _context;

        public System.Random _random = new System.Random(); // Fallback if needed, but we use GUID

        public ShipmentBookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShipmentBooking>> GetBookingsAsync(string role, string associatedName)
        {
            IQueryable<ShipmentBooking> query = _context.ShipmentBookings;

            if (role == UserRoles.Shipper)
            {
                query = query.Where(b => b.ShipperName == associatedName);
            }
            else if (role == UserRoles.Consignee)
            {
                query = query.Where(b => b.ConsigneeName == associatedName);
            }

            return await query.ToListAsync();
        }

        public async Task<ShipmentBooking?> GetBookingDetailsAsync(int id)
        {
            return await _context.ShipmentBookings
                .Include(b => b.Containers)
                .Include(b => b.CustomsDeclarations)
                .Include(b => b.FreightInvoices)
                .FirstOrDefaultAsync(m => m.BookingId == id);
        }

        public async Task<bool> CreateBookingAsync(ShipmentBooking booking, string role, string associatedName)
        {
            if (role == UserRoles.Shipper)
            {
                booking.ShipperName = associatedName ?? "Unknown Shipper";
            }

            booking.BookingNumber = GenerateUniqueBookingNumber();
            booking.BookingStatus = BookingStatus.DRAFT;

            _context.Add(booking);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ConfirmBookingAsync(int id)
        {
            var booking = await _context.ShipmentBookings.FindAsync(id);
            if (booking == null) return false;

            if (booking.BookingStatus == BookingStatus.DRAFT)
            {
                booking.BookingStatus = BookingStatus.CONFIRMED;
                _context.Update(booking);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> AmendBookingAsync(ShipmentBooking booking, string role, string associatedName)
        {
            var existing = await _context.ShipmentBookings.AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingId == booking.BookingId);

            if (existing == null) return false;

            // Security validations inside service layer
            if (role == UserRoles.Shipper && existing.BookingStatus != BookingStatus.DRAFT) return false;

            booking.BookingNumber = existing.BookingNumber;
            if (role == UserRoles.Shipper)
            {
                booking.ShipperName = existing.ShipperName;
                booking.BookingStatus = existing.BookingStatus;
            }

            _context.Update(booking);
            return await _context.SaveChangesAsync() > 0;
        }

        private string GenerateUniqueBookingNumber()
        {
            var timestamp = DateTime.Now.ToString("yyMMdd");
            var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            return $"BKG-{timestamp}-{uniqueId}";
        }
    }
}
