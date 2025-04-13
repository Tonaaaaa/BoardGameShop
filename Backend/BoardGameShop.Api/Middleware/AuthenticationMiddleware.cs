using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Xác thực được thực hiện bởi AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // Middleware này không cần thêm logic vì JwtBearer đã xử lý
            await _next(context);
        }
    }
}