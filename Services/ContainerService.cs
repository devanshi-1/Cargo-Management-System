using System.Collections.Generic;
using Cargo_Management_Project.Models;

namespace Cargo_Management_Project.Services
{
    public class ContainerService : IContainerService
    {
        public IEnumerable<Container> GetAllocatedContainers()
        {
            // Fallback mock dataset matching the blueprint design spec
            return new List<Container>
            {
                new Container { ContainerId = 1, ContainerNumber = "CNTR-DRY-10001", ContainerType = "DRY", BookingId = 1001, SealNumber = "123", ContainerStatus = "LOADED" },
                new Container { ContainerId = 2, ContainerNumber = "CNTR-FLA-10002", ContainerType = "FLAT_RACK", BookingId = 1002, SealNumber = "", ContainerStatus = "EMPTY" }
            };
        }

        public void AssignContainer(Container container)
        {
            // Operational pipeline hook for Day 2 integration
        }

        public void LoadContainer(int containerId, string sealNumber)
        {
            // Operational pipeline hook for Day 2 integration
        }
    }
}
