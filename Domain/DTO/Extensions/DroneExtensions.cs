using devboost.dronedelivery.felipe.DTO.Models;
using System;

namespace devboost.dronedelivery.felipe.DTO.Extensions
{
    public static class DroneExtensions
    {
        private const int TEMPO_CARGA = 1;

        public static DateTime ToTempoGasto(this Drone drone, double distancia)
        {
            var calculo = (distancia / drone.Velocidade);
            return DateTime.Now.AddHours(calculo + TEMPO_CARGA);
        }
    }
}
