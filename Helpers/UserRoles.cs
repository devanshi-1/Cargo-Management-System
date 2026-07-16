namespace Cargo_Management_Project.Helpers
{
    /// <summary>
    /// Constants for user roles across the application.
    /// This centralized definition prevents magic strings and improves maintainability.
    /// </summary>
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Shipper = "Shipper";
        public const string Consignee = "Consignee";
        public const string FreightForwarder = "Freight Forwarder";

        /// <summary>
        /// Get user-friendly display name for a role.
        /// </summary>
        public static string GetDisplayName(string role)
        {
            return role switch
            {
                Admin => "System Administrator",
                Shipper => "Shipper",
                Consignee => "Consignee",
                FreightForwarder => "Freight Forwarder",
                _ => "Unknown Role"
            };
        }
    }
}
