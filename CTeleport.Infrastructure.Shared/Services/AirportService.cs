using CTeleport.Application.Interfaces;
using CTeleport.Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace CTeleport.Infrastructure.Shared.Services
{
    public class AirportService : IAirportService
    {
        private static readonly MemoryCache airportCache = new(new MemoryCacheOptions());
        private static readonly SemaphoreSlim airportCacheLock = new(1, 1);

        public AirportService()
        {
        }

        public async Task<Coordinate> GetCoordinate(string iataCode)
        {
            var coordinate = new Coordinate();

            if (!airportCache.TryGetValue(iataCode, out coordinate))
            {
                await airportCacheLock.WaitAsync();
                try
                {
                    if (!airportCache.TryGetValue(iataCode, out coordinate))
                    {
                        coordinate = new Coordinate(); //get from api

                        airportCache.Set(iataCode, coordinate);
                    }
                }
                finally
                {
                    airportCacheLock.Release();
                }
            }

            return coordinate;
        }
    }
}