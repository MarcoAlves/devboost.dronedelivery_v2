using System.ComponentModel.DataAnnotations;

namespace devboost.dronedelivery.felipe.DTO.Models
{
    public class Drone
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Capacidade deve ser informada!")]
        [Range(1, int.MaxValue, ErrorMessage ="A capacidade minima deve ser 1.")]
        public int Capacidade { get; set; }

        [Required(ErrorMessage = "Velocidade deve ser informada!")]
        [Range(1, int.MaxValue, ErrorMessage = "A velocidade minima deve ser 1.")]
        public int Velocidade { get; set; }

        [Required(ErrorMessage = "Autonomia deve ser informada!")]
        [Range(1, int.MaxValue, ErrorMessage = "A Autonomia minima deve ser 1.")]
        public int Autonomia { get; set; }

        [Required(ErrorMessage = "Carga deve ser informada!")]
        [Range(1, int.MaxValue, ErrorMessage = "A Carga minima deve ser 1.")]
        public int Carga { get; set; }

        [Required(ErrorMessage = "Perfomance deve ser informada!")]
        [Range(1, float.MaxValue, ErrorMessage = "A Perfomance minima deve ser 1.")]
        public float Perfomance { get; set; }
    }
}
