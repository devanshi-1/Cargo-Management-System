using Microsoft.AspNetCore.Mvc;

namespace Cargo_Management_Project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // Mock KPI Statistics
            ViewBag.TotalBookings = 142;
            ViewBag.ActiveContainers = 38;
            ViewBag.CustomsCleared = "94%";
            ViewBag.TotalProfit = 48500.00;

            // Mock Data for Profit Line Graph (Jan - June)
            ViewBag.ProfitMonths = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" };
            ViewBag.ProfitValues = new int[] { 12000, 19000, 15000, 25000, 32000, 48500 };

            // Mock Data for Booking Pie Chart (Statuses)
            ViewBag.BookingStatusLabels = new string[] { "Confirmed", "In Transit", "Completed", "Cancelled" };
            ViewBag.BookingStatusCounts = new int[] { 45, 60, 30, 7 };

            return View();
        }

        // Placeholder actions for User/Role Management (To be hooked up by team later)
        [HttpPost]
        public IActionResult AddUserRole(string username, string role)
        {
            // TODO: Add user role assignment logic
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteUserRole(int id)
        {
            // TODO: Remove user role logic
            return RedirectToAction("Index");
        }
    }
}
