using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace BoardGameShop.Api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestTime = DateTime.UtcNow;
            var requestPath = context.Request.Path;

            Log.Information($"Request started: {requestPath} at {requestTime}");

            await _next(context);

            var statusCode = context.Response.StatusCode;
            Log.Information($"Request completed: {requestPath} with status {statusCode} at {DateTime.UtcNow}");
        }
    }
}