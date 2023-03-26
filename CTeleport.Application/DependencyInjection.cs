using CTeleport.Application.Behavior;
using CTeleport.Application.Features.Distance.Rules;
using CTeleport.Application.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CTeleport.Application
{
    public static class DependencyInjection
    {
        public static void RegisterApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
            services.AddScoped<CoordinateRules>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }

        public static void RegisterApplicationMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<ResponseWrapperMiddleware>();
        }
    }
}