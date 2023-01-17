
using Serilog;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterSentiments.Domain;

namespace TwitterSentiments.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> logger;

        public ApiExceptionMiddleware(RequestDelegate next,ILogger<ApiExceptionMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var error = new ApiError
            {
                Id = Guid.NewGuid().ToString(),
                Status = (short)HttpStatusCode.InternalServerError,
                Title = "Some kind of error occurred in the API."
            };

            var innerExMessage = exception.GetBaseException().Message;

            logger.LogError(exception, "API Error: " + "{ErrorMessage} -- {ErrorId}.", innerExMessage, error.Id);

            var result = JsonSerializer.Serialize(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}
