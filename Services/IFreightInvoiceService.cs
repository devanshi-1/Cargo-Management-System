using System.Threading.Tasks;
using Cargo_Management_Project.Models;

namespace Cargo_Management_Project.Services
{
    /// <summary>
    /// Service interface for freight invoice calculations and management.
    /// Handles ocean freight charges, surcharges, and detention/demurrage fees.
    /// </summary>
    public interface IFreightInvoiceService
    {
        /// <summary>
        /// Calculate ocean freight charges for a shipment.
        /// </summary>
        decimal CalculateOceanFreight(decimal baseFreightCharges);

        /// <summary>
        /// Calculate surcharges (e.g., fuel surcharge, environmental surcharge).
        /// </summary>
        decimal CalculateSurcharges(decimal surchargeAmount);

        /// <summary>
        /// Calculate demurrage fees (container holding charges).
        /// </summary>
        decimal CalculateDemurrage(decimal demurrageAmount);

        /// <summary>
        /// Calculate detention charges (equipment out-of-service fees).
        /// </summary>
        decimal CalculateDetentionCharges(decimal detentionAmount);

        /// <summary>
        /// Create or update a freight invoice asynchronously.
        /// </summary>
        Task<bool> CreateOrUpdateInvoiceAsync(FreightInvoice invoice);
    }
}
