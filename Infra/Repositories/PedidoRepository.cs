using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.EF.Repositories
{
    public class PedidoRepository : RepositoryBase, IPedidoRepository
    {
        protected PedidoRepository(DataContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task SavePedidoAsync(Pedido pedido)
        {
            _context.Pedido.Add(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
