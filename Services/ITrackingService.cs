using System.Threading.Tasks;
using Cargo_Management_Project.Models;

namespace Cargo_Management_Project.Services
{
    public interface ITrackingService
    {
        Task<bool> RecordPortEventAsync(CargoEvent cargoEvent);
    }
}