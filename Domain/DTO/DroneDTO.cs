namespace devboost.dronedelivery.felipe.DTO
{
    public sealed class DroneDto
    {
        public DroneDto(DroneStatusDto droneStatus, double distancia)
        {
            DroneStatus = droneStatus;
            Distancia = distancia;
        }
        public DroneStatusDto DroneStatus { get; set; }

        public double Distancia { get; set; }

    }
}
