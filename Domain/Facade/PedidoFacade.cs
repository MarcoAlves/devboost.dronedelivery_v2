using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.Facade.Interface;
using devboost.dronedelivery.felipe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Facade
{
    public class PedidoFacade : IPedidoFacade
    {
        private readonly DataContext _dataContext;
        private readonly IPedidoService _pedidoService;
        public PedidoFacade(DataContext dataContext, IPedidoService pedidoFacade)
        {
            _dataContext = dataContext;
            _pedidoService = pedidoFacade;

        }
        public async Task AssignDrone()
        {
            var pedidos = await PegaPedidosAsync().ConfigureAwait(false);
            foreach (var pedido in pedidos)
            {
                var drone = await _pedidoService.DroneAtendePedido(pedido);
                await AtualizaPedidoAsync(pedido).ConfigureAwait(false);
                await AdicionarPedidoDrone(pedido, drone).ConfigureAwait(false);

            }
        }

        private async Task AdicionarPedidoDrone(Pedido pedido, DTO.DroneDto drone)
        {
            var pedidoDrone = new PedidoDrone()
            {
                Distancia = drone.Distancia,
                Drone = drone.DroneStatus.Drone,
                DroneId = drone.DroneStatus.Drone.Id,
                Pedido = pedido,
                PedidoId = pedido.Id,
                StatusEnvio = (int)StatusEnvio.AGUARDANDO
            };
            _dataContext.PedidoDrones.Add(pedidoDrone);
            await _dataContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task<Pedido[]> PegaPedidosAsync()
        {
            return await _dataContext.Pedido.Where(FiltraPedidos()).ToArrayAsync().ConfigureAwait(false);
        }

        private async Task AtualizaPedidoAsync(Pedido pedido)
        {
            pedido.Situacao = (int)StatusPedido.AGUARDANDO_ENVIO;
            pedido.DataUltimaAlteracao = DateTime.Now;
            _dataContext.Pedido.Update(pedido);
            await _dataContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private Expression<Func<Pedido, bool>> FiltraPedidos()
        {
            return p => p.Situacao == (int)StatusPedido.AGUARDANDO;
        }
    }
}
