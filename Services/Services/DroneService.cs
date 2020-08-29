using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.DTO.Extensions;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using devboost.dronedelivery.felipe.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services
{
    public class DroneService : IDroneService
    {

        private readonly ICoordinateService _coordinateService;
        private readonly IPedidoDroneRepository _pedidoDroneRepository;
        private readonly IDroneRepository _droneRepository;
        public DroneService(ICoordinateService coordinateService,
            IPedidoDroneRepository pedidoDroneRepository,
            IDroneRepository droneRepository)
        {
            _coordinateService = coordinateService;
            _pedidoDroneRepository = pedidoDroneRepository;
            _droneRepository = droneRepository;
        }


        /// <summary>
        /// Retorna os drones que podem ser utilizados para a entrega
        /// </summary>
        /// <param name="distance">Distancia entre o ponto de entrega e o endereço adicionado</param>
        /// <param name="pedido"></param>
        /// <returns></returns>
        public async Task<DroneStatusDto> GetAvailiableDroneAsync(double distance, Pedido pedido)
        {
            var drones = (await _pedidoDroneRepository.RetornaPedidosEmAberto())
                .Select(d => new
                {
                    distance = _coordinateService.GetKmDistance(d.Pedido.GetPoint(), pedido.GetPoint()),
                    droneId = d.DroneId
                }).OrderBy(p => p.distance);



            if (drones != null)
            {

                foreach (var drone in drones)
                {
                    var resultado = await _droneRepository.RetornaDroneStatus(drone.droneId).ConfigureAwait(false);
                    if (ConsegueCarregar(resultado, drone.distance, distance, pedido))
                    {
                        return resultado;
                    }
                    else
                    {
                        var distanciaPedido = resultado.SomaDistancia + distance + drone.distance;
                        await _pedidoDroneRepository.UpdatePedidoDrone(resultado, distanciaPedido)
                            .ConfigureAwait(false);
                    }
                }
                return null;
            }
            else
            {
                await FinalizaPedidosAsync();
                var drone = _droneRepository.RetornaDrone();
                return new DroneStatusDto(drone);
            }
        }

        public async Task<List<StatusDroneDto>> GetDroneStatusAsync()
        {
            return await _droneRepository.GetDroneStatusAsync();
        }

        private async Task FinalizaPedidosAsync()
        {

            var pedidos = await _pedidoDroneRepository.RetornaPedidosParaFecharAsync();
            if (pedidos.Count > 0)
            {
                foreach (var pedido in pedidos)
                {
                    pedido.StatusEnvio = (int)StatusEnvio.FINALIZADO;
                    await _pedidoDroneRepository.UpdatePedido(pedido);
                }
            }
        }


        private bool ConsegueCarregar(DroneStatusDto droneStatus,
            double PedidoDroneDistance,
            double DistanciaRetorno,
            Pedido pedido)
        {
            return droneStatus != null
                    && (ValidaDistancia(droneStatus, PedidoDroneDistance, DistanciaRetorno))
                    && ValidaPeso(droneStatus, pedido);
        }

        private static bool ValidaPeso(DroneStatusDto droneStatus, Pedido pedido)
        {
            return droneStatus.SomaPeso + pedido.Peso < droneStatus.Drone.Capacidade;
        }

        private static bool ValidaDistancia(DroneStatusDto droneStatus, double PedidoDroneDistance, double DistanciaRetorno)
        {
            return droneStatus.SomaDistancia + DistanciaRetorno + PedidoDroneDistance < droneStatus.Drone.Perfomance;
        }

    }
}
