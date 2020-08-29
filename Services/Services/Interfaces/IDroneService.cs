using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services.Interfaces
{
    public interface IDroneService
    {
        Task<DroneStatusDto> GetAvailiableDroneAsync(double distance, Pedido pedido);
        Task<List<StatusDroneDto>> GetDroneStatusAsync();

    }
}
