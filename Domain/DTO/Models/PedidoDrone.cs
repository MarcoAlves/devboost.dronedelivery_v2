using System;
using System.ComponentModel.DataAnnotations;

namespace devboost.dronedelivery.felipe.DTO.Models
{
    public class PedidoDrone
    {        
        public int Id { get; set; }

        [Required(ErrorMessage = "Drone Id deve ser informado!")]
        public int DroneId { get; set; }

        public Drone Drone { get; set; }

        [Required(ErrorMessage = "Pedido Id deve ser informado!")]
        public int PedidoId { get; set; }
        
        public Pedido Pedido { get; set; }

        [Required(ErrorMessage = "Distancia deve ser informada!")]
        public double Distancia { get; set; }

        [Required(ErrorMessage = "Status Envio deve ser informado!")]
        public int StatusEnvio { get; set; }
        public DateTime DataHoraFinalizacao { get; set; }
    }
}
