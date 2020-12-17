using System;
using System.Net;
using System.Threading.Tasks;
using IdentityServerTemplate.Shared.Infrastructure;
using IdentityServerTemplate.Shared.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace IdentityServerTemplate.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILoggerService _logger;

        public GlobalExceptionHandlerMiddleware(ILoggerService logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await _logger.Error($"Unexpected error: {ex.StackTrace}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(
                JsonConvert.SerializeObject(
                    ApiResponse.InternalServerError("An error occurred whilst processing your request"))); //TODO: message
        }
    }
}