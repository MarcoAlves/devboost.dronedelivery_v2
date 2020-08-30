using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.DTO.Extensions;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using devboost.dronedelivery.felipe.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Services
{
    public class DroneService : IDroneService
    {

        private readonly ICoordinateService _coordinateService;
        private readonly IPedidoDroneRepository _pedidoDroneRepository;
        private readonly IDroneRepository _droneRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public DroneService(ICoordinateService coordinateService,
            IPedidoDroneRepository pedidoDroneRepository,
            IDroneRepository droneRepository,
            IClienteRepository clienteRepository, 
            IPedidoRepository pedidoRepository)
        {
            _coordinateService = coordinateService;
            _pedidoDroneRepository = pedidoDroneRepository;
            _droneRepository = droneRepository;
            _clienteRepository = clienteRepository;
            _pedidoRepository = pedidoRepository;
        }


        /// <summary>
        /// Retorna os drones que podem ser utilizados para a entrega
        /// </summary>
        /// <param name="distance">Distancia entre o ponto de entrega e o endereço adicionado</param>
        /// <param name="pedido"></param>
        /// <returns></returns>
        public async Task<DroneStatusDto> GetAvailiableDroneAsync(double distance, Pedido pedido)
        {

            var drones = new List<DroneCalculoDTO>();
            var pedidosAbertos = (await _pedidoDroneRepository.RetornaPedidosEmAberto());

            foreach (var item in pedidosAbertos)
            {
                var pedidoTemp = await _pedidoRepository.GetById(item.PedidoId);
                var cliente = await _clienteRepository.GetById(pedidoTemp.ClienteId);
                var origemPoint = new Point(cliente.Latitude, cliente.Longitude);

                var clientePedidoAtual = await _clienteRepository.GetById(pedidoTemp.ClienteId);
                var destinoPoint = new Point(clientePedidoAtual.Latitude, clientePedidoAtual.Longitude);

                drones.Add(new DroneCalculoDTO
                {
                    Distancia = _coordinateService.GetKmDistance(origemPoint, destinoPoint),
                    DroneId = item.DroneId
                });

            }

            drones = drones.OrderBy(p => p.Distancia).ToList();

            if (drones.Count() > 0)
            {

                foreach (var drone in drones)
                {
                    var resultado = await _droneRepository.RetornaDroneStatus(drone.DroneId).ConfigureAwait(false);
                    if (ConsegueCarregar(resultado, drone.Distancia, distance, pedido))
                    {
                        return resultado;
                    }
                    else
                    {
                        var distanciaPedido = resultado.SomaDistancia + distance + drone.Distancia;
                        await _pedidoDroneRepository.UpdatePedidoDrone(resultado, distanciaPedido)
                            .ConfigureAwait(false);
                    }
                }
                return null;
            }
            else
            {
                await FinalizaPedidosAsync();
                var drone = _droneRepository.RetornaDroneApto(distance, pedido.Peso);
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
