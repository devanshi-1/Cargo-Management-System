using Cargo_Management_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargo_Management_Project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ShipmentBooking> ShipmentBookings { get; set; }
        public DbSet<Container> Containers { get; set; }

        public DbSet<FreightInvoice> FreightInvoices { get; set; }

    }
}
