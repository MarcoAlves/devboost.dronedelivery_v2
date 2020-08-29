using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Extensions;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.Services.Interfaces;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IDroneService _droneService;
        private readonly ICoordinateService _coordinateService;
        public PedidoService(IDroneService droneService, ICoordinateService coordinateService)
        {
            _droneService = droneService;
            _coordinateService = coordinateService;
        }

        public async Task<DroneDto> DroneAtendePedido(Pedido pedido)
        {
            var originPoint = new Point();

            var distance = _coordinateService.GetKmDistance(originPoint, pedido.GetPoint());


            var drone = await _droneService.GetAvailiableDroneAsync(distance, pedido).ConfigureAwait(false);
            if (drone == null)
                return null;

            return new DroneDto(drone, distance);

        }

    }
}
