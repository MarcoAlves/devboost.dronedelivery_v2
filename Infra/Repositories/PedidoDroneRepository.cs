using Dapper;
using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.DTO.Extensions;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.EF.Repositories
{
    public class PedidoDroneRepository : RepositoryBase, IPedidoDroneRepository
    {
        public PedidoDroneRepository(DataContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task<List<PedidoDrone>> RetornaPedidosEmAberto()
        {
            return await _context.PedidoDrones.Where(FiltroPedidosEmAberto()).ToListAsync();
        }

        private static Expression<Func<PedidoDrone, bool>> FiltroPedidosEmAberto()
        {
            return p => p.StatusEnvio == (int)StatusEnvio.AGUARDANDO;
        }

        public async Task UpdatePedidoDrone(DroneStatusDto drone, double distancia)
        {
            using SqlConnection conexao = new SqlConnection(_connectionString);
            await conexao.ExecuteAsync("UPDATE dbo.PedidoDrones" +
                $" SET StatusPedido ={(int)StatusEnvio.EM_TRANSITO}," +
                $"DataHoraFinalizacao = {drone.Drone.ToTempoGasto(distancia)}" +
                $" WHERE DroneId = {drone.Drone.Id}")
                .ConfigureAwait(false);
        }

        public async Task<List<PedidoDrone>> RetornaPedidosParaFecharAsync()
        {
            return await _context
                .PedidoDrones
                .Where(p =>
                    p.StatusEnvio == (int)StatusEnvio.EM_TRANSITO &&
                    p.DataHoraFinalizacao <= DateTime.Now)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task UpdatePedido(PedidoDrone pedido)
        {
            _context.PedidoDrones.Update(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
