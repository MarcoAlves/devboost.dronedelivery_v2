using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.EF.Repositories.Interfaces
{
    public interface IDroneRepository
    {
        Task<List<StatusDroneDto>> GetDroneStatusAsync();
        Task<DroneStatusDto> RetornaDroneStatus(int droneId);
        Drone RetornaDrone();
        Task SaveDrone(Drone drone);


    }
}
