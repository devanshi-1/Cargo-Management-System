using Microsoft.AspNetCore.Mvc;
using Cargo_Management_Project.Models;
using System.Collections.Generic;

namespace Cargo_Management_Project.Controllers
{
    public class ContainerController : Controller
    {
        // GET: /Container/Index
        public IActionResult Index()
        {
            // We create MOCK (fake) data so your front-end works instantly
            var mockContainers = new List<Container>
            {
                new Container { ContainerId = 1, ContainerNumber = "CNTR-DRY-10001", ContainerType = ContainerType.DRY, BookingId = 1001, SealNumber = "123", ContainerStatus = ContainerStatus.LOADED },
                new Container { ContainerId = 2, ContainerNumber = "CNTR-FLA-10002", ContainerType = ContainerType.FLAT_RACK, BookingId = 1002, SealNumber = "", ContainerStatus = ContainerStatus.EMPTY }
            };

            var viewModel = new ContainerDashboardViewModel
            {
                AllocatedContainers = mockContainers
            };

            return View(viewModel);
        }

        // POST: /Container/AssignContainer
        [HttpPost]
        public IActionResult AssignContainer(int BookingId, string ContainerType, string ContainerNumber)
        {
            // For now, just reload the page when they click "Allocate Equipment"
            return RedirectToAction("Index");
        }

        // POST: /Container/LoadContainer
        [HttpPost]
        public IActionResult LoadContainer(int ContainerId, string SealNumber)
        {
            // For now, just reload the page when they click "Seal"
            return RedirectToAction("Index");
        }

        // POST: /Container/GenerateManifest
        [HttpPost]
        public IActionResult GenerateManifest(int voyageId)
        {
            // Placeholder logic for service tracking integration
            return RedirectToAction("Index");
        }
    }
}