using CTeleport.Domain.Common;

namespace CTeleport.Application.Interfaces
{
    public interface IAirportService
    {
        Task<Coordinate> GetCoordinate(string iataCode);
    }
}