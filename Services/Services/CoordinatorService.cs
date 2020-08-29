using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.Services.Interfaces;
using Geolocation;

namespace devboost.dronedelivery.felipe.Services
{
    public class CoordinateService : ICoordinateService
    {
        public double GetKmDistance(Point originPoint, Point destPoint)
        {
            return GeoCalculator.GetDistance(
                originPoint.Latitude,
                originPoint.Longitude,
                destPoint.Latitude,
                destPoint.Longitude,
                1,
                DistanceUnit.Kilometers);
        }
    }
}
