using Microsoft.AspNetCore.Localization;

namespace CTeleport.API.Helper
{
    public class CultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var acceptLanguage = httpContext.Request.GetTypedHeaders().AcceptLanguage.FirstOrDefault()?.Value.Value ?? "en";

            return new ProviderCultureResult(acceptLanguage);
        }
    }
}