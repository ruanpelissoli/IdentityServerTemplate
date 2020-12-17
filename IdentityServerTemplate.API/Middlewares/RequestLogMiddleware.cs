using IdentityServerTemplate.Core.DTOs.Logs;
using IdentityServerTemplate.Shared.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace IdentityServerTemplate.API.Middlewares
{
    public class RequestLogMiddleware : IMiddleware
    {
        public ILoggerService _logger;
        public IIdentityUserService _identityUserService;

        public RequestLogMiddleware(ILoggerService logger, IIdentityUserService identityUserService)
        {
            _logger = logger;
            _identityUserService = identityUserService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                using var stream = new StreamReader(context.Request.Body);

                var body = await stream.ReadToEndAsync();

                var requestLogDTO = new RequestLogDTO
                {
                    UserId = _identityUserService.Id,
                    UserName = _identityUserService.UserName,
                    HttpMethod = context.Request.Method,
                    ApiRoute = context.Request.Path,
                    QueryParams = context.Request.QueryString.Value,
                    RequestBody = body,
                    IpAddress = context.Request.HttpContext.Connection.RemoteIpAddress.ToString()
                };

                await _logger.Info(requestLogDTO);
            }
            catch (System.Exception ex)
            {
                await _logger.Error(ex.StackTrace);
            }            
        }
    }
}
