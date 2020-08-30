using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Extensions;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using devboost.dronedelivery.felipe.Services.Interfaces;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IDroneService _droneService;
        private readonly ICoordinateService _coordinateService;
        private readonly IClienteRepository _clienteRepository;
        public PedidoService(IDroneService droneService, ICoordinateService coordinateService, IClienteRepository clienteRepository)
        {
            _droneService = droneService;
            _coordinateService = coordinateService;
            _clienteRepository = clienteRepository;
        }

        public async Task<DroneDto> DroneAtendePedido(Pedido pedido)
        {
            var originPoint = new Point();
            var cliente = await _clienteRepository.GetById(pedido.ClienteId);

            var destinationPoint =  new Point(cliente.Latitude, cliente.Longitude);

            var distance = _coordinateService.GetKmDistance(originPoint, destinationPoint);

            var drone = await _droneService.GetAvailiableDroneAsync(distance, pedido).ConfigureAwait(false);
            if (drone == null)
                return null;

            return new DroneDto(drone, distance);

        }

    }
}
