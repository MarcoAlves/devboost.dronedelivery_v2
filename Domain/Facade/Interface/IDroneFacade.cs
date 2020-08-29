using devboost.dronedelivery.felipe.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Facade.Interface
{
    public interface IDroneFacade
    {
        Task<List<StatusDroneDto>> GetDroneStatusAsync();


    }
}
