using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BoardGameShop.Api.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationMiddleware> _logger;

        public AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing or invalid token.");
                return;
            }

            var userRole = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value; // Sửa từ "role" thành ClaimTypes.Role
            var roleAttribute = endpoint?.Metadata?.GetMetadata<AuthorizeRoleAttribute>();
            if (roleAttribute != null && !roleAttribute.Roles.Contains(userRole))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("User does not have the required role.");
                return;
            }

            await _next(context);
        }
    }

    public class AuthorizeRoleAttribute : Attribute
    {
        public string[] Roles { get; }

        public AuthorizeRoleAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }

    public class AllowAnonymousAttribute : Attribute
    {
    }

    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}