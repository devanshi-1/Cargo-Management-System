using CargoManagementSystem.Models;

namespace Cargo_Management_Project.Services
{
    public interface ICustomsService
    {
        /// <summary>
        /// Applies the Harmonized System (HS) code to the customs declaration.
        /// </summary>
        void ApplyHsCode(CustomsDeclaration declaration, string hsCode);

        /// <summary>
        /// Calculates the tariff rate based on the declared value and HS code.
        /// </summary>
        decimal CalculateTariffRate(decimal declaredValue, string hsCode);

        /// <summary>
        /// Validates the documentation integrity of the customs declaration.
        /// </summary>
        bool ValidateDocumentation(CustomsDeclaration declaration);
    }
}
