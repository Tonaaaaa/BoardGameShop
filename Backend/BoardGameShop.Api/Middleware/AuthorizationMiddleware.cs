using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Quyền truy cập được kiểm soát bởi [Authorize(Roles = "Admin")] trong Controllers
            // Middleware này có thể dùng để kiểm tra bổ sung nếu cần
            await _next(context);
        }
    }
}