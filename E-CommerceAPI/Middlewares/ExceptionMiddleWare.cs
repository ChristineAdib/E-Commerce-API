using E_CommerceAPI.Errors;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace E_CommerceAPI.Middlewares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleWare(RequestDelegate Next,ILogger<ExceptionMiddleWare> logger,IHostEnvironment environment)
        {
            _next = Next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                
                var Response = _environment.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString()) : new ApiExceptionResponse(500);
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonResponse = JsonSerializer.Serialize(Response,Options);
                context.Response.WriteAsync(JsonResponse);
            }
        }
    }
}
