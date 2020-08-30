using Dapper;
using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Constants;
using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.EF.Repositories
{
    public class DroneRepository : IDroneRepository
    {
        private readonly DataContext _context;
        private readonly string _connectionString;
        private readonly IClienteRepository _clienteRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public DroneRepository(DataContext context,
            IConfiguration configuration,
            IClienteRepository clienteRepository,
            IPedidoRepository pedidoRepository)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(ProjectConsts.CONNECTION_STRING_CONFIG);
            _clienteRepository = clienteRepository;
            _pedidoRepository = pedidoRepository;

        }

        public async Task SaveDrone(Drone drone)
        {
            _context.Drone.Add(drone);
            await _context.SaveChangesAsync();
        }

        public Drone RetornaDrone()
        {
            return _context.Drone.FirstOrDefault();
        }
        public Drone RetornaDroneApto(double distancia, int peso)
        {
            return _context.Drone.Where(_ => _.Perfomance > distancia && _.Capacidade > peso).FirstOrDefault();
        }

        public async Task<List<StatusDroneDto>> GetDroneStatusAsync()
        {

            using SqlConnection conexao = new SqlConnection(_connectionString);
            var resultado = await conexao.QueryAsync<StatusDroneDto>(GetStatusSqlCommand()).ConfigureAwait(false);

            foreach (var item in resultado)
            {
                var pedido = await _pedidoRepository.GetById(item.PedidoId);
                if(pedido != null)
                {
                    var cliente = await _clienteRepository.GetById(pedido.ClienteId);

                    item.Cliente = new ClienteDTO();
                    item.Cliente.Id = cliente.Id;
                    item.Cliente.Nome = cliente.Nome;
                    item.Cliente.Coordenada = cliente.Latitude.ToString() + ", " + cliente.Longitude; 
                }

            }
            return resultado.ToList();
        }
        public async Task<DroneStatusDto> RetornaDroneStatus(int droneId)
        {
            using SqlConnection conexao = new SqlConnection(_connectionString);
            return (await conexao.QueryAsync<DroneStatusDto>(GetSqlCommand(droneId))
                .ConfigureAwait(false)).FirstOrDefault();
        }

        private string GetSelectPedidos(int situacao, StatusEnvio status)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("select a.DroneId,");
            stringBuilder.AppendLine($"{situacao} as Situacao,");
            stringBuilder.AppendLine("a.Id as PedidoId");
            stringBuilder.AppendLine(" from PedidoDrones a");
            stringBuilder.AppendLine($" where a.StatusEnvio <> {(int)status}");
            stringBuilder.AppendLine(" and a.DataHoraFinalizacao > dateadd(hour,-3,CURRENT_TIMESTAMP)");
            return stringBuilder.ToString();
        }

        private string GetStatusSqlCommand()
        {

            var sql = @"
                SELECT 
                Drone.Id as 'droneId',

                (CASE 
	                WHEN StatusEnvio = 0 THEN 'AGUARDANDO'
	                WHEN StatusEnvio = 1 THEN 'EM TRANSITO'
	                WHEN StatusEnvio = 2 THEN 'FINALIZADO'
	                ELSE ''
                END) as 'Situacao',
                PedidoId,
                Cliente.Id as 'Cliente.Id',
                Cliente.Nome as 'Cliente.Nome',
                CONVERT(varchar(20), Cliente.Latitude) + ', ' + CONVERT(varchar(20), Cliente.Longitude) as 'Cliente.Coordenada'


                FROM Drone
                LEFT JOIN PedidoDrones ON PedidoDrones.DroneId = Drone.Id
                LEFT JOIN Pedido ON Pedido.Id = PedidoDrones.PedidoId
                LEFT JOIN Cliente ON Cliente.Id = Pedido.ClienteId";

            return sql;

        }

        private static string GetSqlCommand(int droneId)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT D.*");
            stringBuilder.AppendLine("SUM(P.Peso) AS SomaPeso,");
            stringBuilder.AppendLine("SUM(PD.Distancia) AS SomaDistancia ");
            stringBuilder.AppendLine("FROM dbo.PedidoDrones PD ");
            stringBuilder.AppendLine("JOIN dbo.Drone D");
            stringBuilder.AppendLine("on PD.DroneId = D.Id");
            stringBuilder.AppendLine("JOIN dbo.Pedido P");
            stringBuilder.AppendLine("on PD.DroneId = P.Id");
            stringBuilder.AppendLine($"WHERE PD.DroneId = {droneId}");
            stringBuilder.AppendLine("GROUP BY D.Id, D.Autonomia, D.Capacidade, D.Carga, D.Perfomance, D.Velocidade");
            return stringBuilder.ToString();
        }

        public Task<List<Drone>> GetAll()
        {
            var drones = _context.Drone.ToListAsync();

            return drones;
        }
    }
}
