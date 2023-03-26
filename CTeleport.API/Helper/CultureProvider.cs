using Microsoft.AspNetCore.Localization;

namespace CTeleport.API.Helper
{
    public class CultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var acceptLanguage = httpContext.Request.GetTypedHeaders().AcceptLanguage.FirstOrDefault()?.Value.Value ?? "en";

            return Task.FromResult(new ProviderCultureResult(acceptLanguage));
        }
    }
}