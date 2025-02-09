using System.Text.Json;

namespace WebAPI.ProductAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogException(context, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private void LogException(HttpContext context, Exception exception)
        {
            var requestId = context.TraceIdentifier;
            var userId = context.User.Identity?.Name ?? "Anonymous";

            _logger.LogError(exception, "Unhandled exception occurred. RequestId: {RequestId}, UserId: {UserId}", requestId, userId);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = GetStatusCodeAndMessage(exception);
            var payload = BuildResponsePayload(context, exception, message, statusCode);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(payload);
        }

        private (int statusCode, string message) GetStatusCodeAndMessage(Exception exception)
        {
            int statusCode = StatusCodes.Status500InternalServerError;
            string message = "An unexpected error occurred.";

            if (exception is KeyNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                message = "Resource not found.";
            }
            else if (exception is ArgumentException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                message = "Invalid input provided.";
            }

            return (statusCode, message);
        }

        private string BuildResponsePayload(HttpContext context, Exception exception, string message, int statusCode)
        {
            var response = new
            {
                message = _env.IsDevelopment() ? exception.Message : message,
                requestId = context.TraceIdentifier,
                statusCode,
                stackTrace = _env.IsDevelopment() ? exception.StackTrace : null
            };

            return JsonSerializer.Serialize(response);
        }
    }
}
