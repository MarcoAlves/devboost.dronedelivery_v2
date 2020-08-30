using devboost.dronedelivery.felipe.DTO.Models;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Facade.Interface
{
    public interface IClienteFacade
    {

        Task Save(Cliente cliente);
        Task Delete(int clienteId);


    }
}
