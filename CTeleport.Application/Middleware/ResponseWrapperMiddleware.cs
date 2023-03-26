using Microsoft.AspNetCore.Http;

namespace CTeleport.Application.Middleware
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //prevent NetworkError 
            context.Response.Headers.Add("Access-Control-Allow-Origin", "https://app.swaggerhub.com");
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            await _next(context);
        }
    }
}
