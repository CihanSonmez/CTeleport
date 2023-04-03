using CTeleport.Application.Interfaces;
using CTeleport.Domain.Common;
using CTeleport.Infrastructure.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace CTeleport.Infrastructure.Services
{
    public class AirportService : IAirportService
    {
        private static readonly MemoryCache airportCache = new(new MemoryCacheOptions());
        private static readonly SemaphoreSlim airportCacheLock = new(1, 1);
        private readonly AirportApiSettings airportApiSettings;

        public AirportService(IOptionsSnapshot<AirportApiSettings> airportApiOptions)
        {
            airportApiSettings = airportApiOptions.Value;
        }

        public async Task<Coordinate> GetCoordinate(string iataCode)
        {
            if (!airportCache.TryGetValue(iataCode, out Coordinate coordinate))
            {
                await airportCacheLock.WaitAsync();
                try
                {
                    if (!airportCache.TryGetValue(iataCode, out coordinate))
                    {
                        using var client = new RestClient(airportApiSettings.BaseUrl);

                        var request = new RestRequest(string.Format(airportApiSettings.GetAirportPath, iataCode.ToUpperInvariant()));

                        var serviceResponse = await client.GetAsync(request);

                        if (serviceResponse.IsSuccessStatusCode)
                        {
                            var responseModel = JsonConvert.DeserializeObject<AirportResponseModel>(serviceResponse.Content);
                            
                            coordinate = new Coordinate()
                            {
                                Latitude = responseModel.Location.Latitude.Value,
                                Longitude = responseModel.Location.Longitude.Value,
                            };

                            airportCache.Set(iataCode, coordinate);
                        }
                        else
                        {
                            throw new Exception($"Code => {iataCode} Error => { serviceResponse.ErrorException?.Message }");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
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