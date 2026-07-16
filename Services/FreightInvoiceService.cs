using System;
using System.Threading.Tasks;
using Cargo_Management_Project.Data;
using Cargo_Management_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Cargo_Management_Project.Services
{
    /// <summary>
    /// Implementation of freight invoice service for calculating and managing shipping charges.
    /// </summary>
    public class FreightInvoiceService : IFreightInvoiceService
    {
        private readonly AppDbContext _context;

        public FreightInvoiceService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Calculate ocean freight charges based on base freight amount.
        /// </summary>
        public decimal CalculateOceanFreight(decimal baseFreightCharges)
        {
            if (baseFreightCharges < 0)
                throw new ArgumentException("Ocean freight charges cannot be negative.", nameof(baseFreightCharges));

            return baseFreightCharges;
        }

        /// <summary>
        /// Calculate surcharges (fuel, environmental, etc.).
        /// </summary>
        public decimal CalculateSurcharges(decimal surchargeAmount)
        {
            if (surchargeAmount < 0)
                throw new ArgumentException("Surcharge amount cannot be negative.", nameof(surchargeAmount));

            return surchargeAmount;
        }

        /// <summary>
        /// Calculate demurrage fees for container holding at terminal.
        /// </summary>
        public decimal CalculateDemurrage(decimal demurrageAmount)
        {
            if (demurrageAmount < 0)
                throw new ArgumentException("Demurrage amount cannot be negative.", nameof(demurrageAmount));

            return demurrageAmount;
        }

        /// <summary>
        /// Calculate detention charges for equipment out-of-service.
        /// </summary>
        public decimal CalculateDetentionCharges(decimal detentionAmount)
        {
            if (detentionAmount < 0)
                throw new ArgumentException("Detention amount cannot be negative.", nameof(detentionAmount));

            return detentionAmount;
        }

        /// <summary>
        /// Create or update a freight invoice in the database.
        /// </summary>
        public async Task<bool> CreateOrUpdateInvoiceAsync(FreightInvoice invoice)
        {
            try
            {
                if (invoice.InvoiceId == 0)
                {
                    // New invoice
                    _context.FreightInvoices.Add(invoice);
                }
                else
                {
                    // Update existing invoice
                    _context.FreightInvoices.Update(invoice);
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Log exception here if logging is configured
                throw new InvalidOperationException("Failed to save freight invoice.", ex);
            }
        }
    }
}
