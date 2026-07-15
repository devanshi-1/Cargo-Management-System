
﻿using Cargo_Management_Project.Models;
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


        public DbSet<CustomsDeclaration> CustomsDeclarations { get; set; }
        public DbSet<CargoEvent> CargoEvents { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Seed a completed Booking
            modelBuilder.Entity<ShipmentBooking>().HasData(
                new ShipmentBooking
                {
                    BookingId = 1,
                    BookingNumber = "BKG-2026-001",
                    ShipperName = "Global Tech Exports",
                    ConsigneeName = "Euro Imports Ltd",
                    OriginPort = "Shanghai",
                    DestinationPort = "Rotterdam",
                    BookingStatus = BookingStatus.COMPLETED
                }
            );

            // 2. Seed a loaded Container linked to the booking
            modelBuilder.Entity<Container>().HasData(
                new Container
                {
                    ContainerId = 1,
                    ContainerNumber = "CONT-998877",
                    ContainerType = ContainerType.DRY,
                    BookingId = 1,
                    SealNumber = "SEAL-XYZ123",
                    ContainerStatus = ContainerStatus.LOADED
                }
            );

            // 3. Seed a cleared Customs Declaration
            modelBuilder.Entity<CustomsDeclaration>().HasData(
                new CustomsDeclaration
                {
                    DeclarationId = 1,
                    BookingId = 1,
                    HsCode = "8471.30.00",
                    DeclaredValue = 50000.00m,
                    CalculatedDuty = 2500.00m,
                    ClearanceStatus = ClearanceStatus.CLEARED
                }
            );

            // 4. Seed a Cargo Event tracking milestone
            modelBuilder.Entity<CargoEvent>().HasData(
                new CargoEvent
                {
                    EventId = 1,
                    ContainerId = 1,
                    EventType = EventType.LOADED,
                    EventLocation = "Port of Shanghai",
                    EventTimestamp = new DateTime(2026, 7, 10, 8, 30, 0),
                    Remarks = "Loaded securely on vessel."
                }
            );

            // 5. Seed a Paid Freight Invoice
            modelBuilder.Entity<FreightInvoice>().HasData(
                new FreightInvoice
                {
                    InvoiceId = 1,
                    BookingId = 1,
                    FreightCharges = 3500.00m,
                    SurchargeAmount = 500.00m,
                    TotalAmount = 4000.00m,
                    Currency = "USD",
                    InvoiceStatus = InvoiceStatus.PAID
                }
            );
        }

    }
}
