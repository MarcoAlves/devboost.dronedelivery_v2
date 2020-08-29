using Dapper;
using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Constants;
using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
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

        public DroneRepository(DataContext context,
            IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(ProjectConsts.CONNECTION_STRING_CONFIG);

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

        public async Task<List<StatusDroneDto>> GetDroneStatusAsync()
        {

            using SqlConnection conexao = new SqlConnection(_connectionString);
            var resultado = await conexao.QueryAsync<StatusDroneDto>(GetStatusSqlCommand()).ConfigureAwait(false);
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
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(GetSelectPedidos(0, StatusEnvio.AGUARDANDO));
            stringBuilder.AppendLine(" union");
            stringBuilder.Append(GetSelectPedidos(1, StatusEnvio.EM_TRANSITO));
            stringBuilder.AppendLine(" union");
            stringBuilder.AppendLine(" select b.Id as DroneId,");
            stringBuilder.AppendLine(" 1 as Situacao,");
            stringBuilder.AppendLine(" 0 as PedidoId");
            stringBuilder.AppendLine(" from  Drone b");
            stringBuilder.AppendLine(" where b.Id NOT IN  (");
            stringBuilder.AppendLine(" select a.DroneId");
            stringBuilder.AppendLine(" from PedidoDrones a");
            stringBuilder.AppendLine($" where a.StatusEnvio <> {(int)StatusEnvio.FINALIZADO}");
            stringBuilder.AppendLine(" and a.DataHoraFinalizacao > dateadd(hour,-3,CURRENT_TIMESTAMP)");
            stringBuilder.AppendLine(")");
            return stringBuilder.ToString();
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





    }
}
