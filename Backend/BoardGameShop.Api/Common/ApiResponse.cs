using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Common
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public ApiResponse(int status, string message, T data = default, Dictionary<string, string> errors = null)
        {
            Status = status;
            Message = message;
            Data = data;
            Errors = errors ?? new Dictionary<string, string>();
        }

        // Phương thức tiện ích để tạo phản hồi thành công
        public static ApiResponse<T> Success(T data, string message = "Success")
        {
            return new ApiResponse<T>(200, message, data);
        }

        // Phương thức tiện ích để tạo phản hồi lỗi
        public static ApiResponse<T> Error(int status, string message, Dictionary<string, string> errors = null)
        {
            return new ApiResponse<T>(status, message, default, errors);
        }
    }
}