using devboost.dronedelivery.felipe.DTO.Models;

namespace devboost.dronedelivery.felipe.DTO
{
    public sealed class DroneStatusDto
    {
        public DroneStatusDto(Drone drone)
        {
            Drone = drone;
            SomaPeso = default;
            SomaDistancia = default;
        }
        public Drone Drone { get; set; }
        public int SomaPeso { get; set; }
        public int SomaDistancia { get; set; }
    }
}
