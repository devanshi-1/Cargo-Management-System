using System.Collections.Generic;
using Cargo_Management_Project.Models;

namespace Cargo_Management_Project.Services
{
    public interface IContainerService
    {
        IEnumerable<Container> GetAllocatedContainers();
        void AssignContainer(Container container);
        void LoadContainer(int containerId, string sealNumber);
    }
}