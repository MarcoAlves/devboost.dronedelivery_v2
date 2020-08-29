using System;
using System.ComponentModel.DataAnnotations;

namespace devboost.dronedelivery.felipe.DTO.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Peso deve ser informado!")]
        [Range(1, int.MaxValue, ErrorMessage = "A Peso minimo deve ser 1.")]
        public int Peso { get; set; }

        [Required(ErrorMessage = "Latitude deve ser informada!")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude deve ser informada!")]
        public double Longitude { get; set; }
        
        public DateTime DataHoraInclusao { get; set; }

        [Required(ErrorMessage = "Situacao deve ser informada!")]
        public int Situacao { get; set; }
        
        public DateTime DataUltimaAlteracao { get; set; }
        public DateTime DataHoraFinalizacao { get; set; }
    }
}
