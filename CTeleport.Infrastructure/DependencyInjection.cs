using CTeleport.Application.Interfaces;
using CTeleport.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CTeleport.Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterInfrastructure(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IAirportService, AirportService>();
        }
    }
}