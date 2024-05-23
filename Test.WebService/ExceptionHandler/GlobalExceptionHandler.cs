using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Test.Core.Logger;

namespace Test.WebService.ExceptionHandler
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly Core.Logger.ILogger _logger;

        public GlobalExceptionHandler(Core.Logger.ILogger logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError($"{exception.Message}\n{exception.StackTrace}");

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
