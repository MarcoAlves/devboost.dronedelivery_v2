using devboost.dronedelivery.felipe.DTO.Models;

namespace devboost.dronedelivery.felipe.DTO
{
    public sealed class StatusDroneDto
    {
        public int DroneId { get; set; }
        public string Situacao { get; set; }
        public int PedidoId { get; set; }

        public ClienteDTO Cliente { get; set; }

    }
}
