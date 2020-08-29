using devboost.dronedelivery.felipe.DTO.Models;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.EF.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task SavePedidoAsync(Pedido pedido);

    }
}
