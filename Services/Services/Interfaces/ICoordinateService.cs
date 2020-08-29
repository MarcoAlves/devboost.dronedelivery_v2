using devboost.dronedelivery.felipe.DTO;

namespace devboost.dronedelivery.felipe.Services.Interfaces
{
    public interface ICoordinateService
    {
        double GetKmDistance(Point originPoint, Point destPoint);
    }
}
