using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using devboost.dronedelivery.felipe.Facade.Interface;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Facade
{
    public class ClienteFacade : IClienteFacade
    {
        
        private readonly IClienteRepository _clienteRepository;

        public ClienteFacade(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public Task Delete(int clienteId)
        {
            return _clienteRepository.Delete(clienteId);
        }

        public Task Save(Cliente cliente)
        {
            
            return _clienteRepository.Save(cliente);

        }

    }
}
