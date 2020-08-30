using devboost.dronedelivery.felipe.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.EF.Repositories.Interfaces
{
    public interface IClienteRepository
    {

        Task Save(Cliente cliente);
        Task Delete(int clienteId);



        Task<List<Cliente>> GetAll();
        Task<Cliente> GetById(int clienteId);
        Task<Cliente> GetByName(string nome);
 
    }
}
