using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            switch (exception)
            {
                case NotFoundException:
                    _logger.LogWarning(
                        "Resource not found while processing {Method} {Path}: {Message}",
                        httpContext.Request.Method,
                        httpContext.Request.Path,
                        exception.Message);
                    break;

                case RequestValidationException:
                    _logger.LogWarning(
                        "Request validation failed while processing {Method} {Path}: {Message}",
                        httpContext.Request.Method,
                        httpContext.Request.Path,
                        exception.Message);
                    break;

                default:
                    _logger.LogError(
                        exception,
                        "Unhandled exception occurred while processing {Method} {Path}",
                        httpContext.Request.Method,
                        httpContext.Request.Path);
                    break;
            }

            var problemDetails = exception switch
            {
                NotFoundException => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = exception.Message
                },

                RequestValidationException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = exception.Message
                },

                _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred."
                }
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
            throw new NotImplementedException();
        }
    }
}
