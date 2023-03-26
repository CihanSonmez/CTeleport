using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace CTeleport.Localization
{
    public static class DependencyInjection
    {
        public static void RegisterLocalization(this IServiceCollection services)
        {
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("en"),
                    new CultureInfo("nl"),
                    new CultureInfo("de"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddScoped<Localizer>();
        }

        public static void ConfigureLocalization(this IApplicationBuilder app, RequestCultureProvider cultureProvider)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            options.Value.RequestCultureProviders.Clear();
            options.Value.RequestCultureProviders.Add(cultureProvider);
            app.UseRequestLocalization(options.Value);

            Localizer.ResourceCompatibilityCheck();
        }
    }
}