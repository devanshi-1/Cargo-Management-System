namespace Cargo_Management_Project.Services
{
    public interface IFreightInvoiceService
    {
        decimal OceanFreight(decimal freightcharges);
        decimal Surcharges(decimal surchargeAmount);

        decimal Demurrage(decimal demurrageAmount);

        decimal DetentionCharges(decimal detentionAmount);
        public class FreightInvoiceService : IFreightInvoiceService
        {
            public decimal OceanFreight(decimal freightcharges)
            {
                return freightcharges;
            }
            public decimal Surcharges(decimal surchargeAmount)
            {
                return surchargeAmount;
            }
            public decimal Demurrage(decimal demurrageAmount)
            {
                return demurrageAmount;
            }
            public decimal DetentionCharges(decimal detentionAmount)
            {
                return detentionAmount;
            }
        }
    }
}
