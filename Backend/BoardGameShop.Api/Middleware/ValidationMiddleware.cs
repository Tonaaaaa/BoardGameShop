using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // FluentValidation tự động kiểm tra DTO nhờ AddFluentValidationAutoValidation()
            // Middleware này có thể dùng để kiểm tra bổ sung nếu cần
            await _next(context);
        }
    }
}