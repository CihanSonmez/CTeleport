using CTeleport.Application.Wrappers;
using CTeleport.Domain.Enums;
using CTeleport.Domain.Exceptions;
using CTeleport.Localization;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace CTeleport.Application.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, Localizer localizer, ILogger<string> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception, localizer, logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context,
            Exception exception,
            Localizer localizer,
            ILogger<string> logger)
        {
            context.Response.ContentType = "application/json";

            if (exception.GetType() == typeof(ValidationException))
                await HandleValidationException(context, (ValidationException)exception);

            else if (exception.GetType() == typeof(BusinessException))
                await HandleBusinessException(context, (BusinessException)exception);

            else
                await HandleInternalException(context, exception, localizer, logger);
        }

        private async Task HandleValidationException(HttpContext context, ValidationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            StringBuilder sb = new StringBuilder();
            var errorsMessages = exception.Errors.Select(x => x.ErrorMessage);
            sb.AppendJoin(' ', errorsMessages);

            var response = Response.Fail(ErrorType.Validation, sb.ToString());
            var responseString = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(responseString);

            return;
        }

        private async Task HandleBusinessException(HttpContext context, BusinessException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            var response = Response.Fail(exception.Type, exception.Message);
            var responseString = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(responseString);

            return;
        }

        private async Task HandleInternalException(HttpContext context,
            Exception exception,
            Localizer localizer,
            ILogger<string> logger)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            string message = $"Error Message: {exception.Message}\n StackTrace: {exception.StackTrace}";
            logger.LogCritical(message);

            var response = Response.Fail(ErrorType.Internal, localizer.AnErrorOccured);
            var responseString = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(responseString);

            return;
        }
    }
}