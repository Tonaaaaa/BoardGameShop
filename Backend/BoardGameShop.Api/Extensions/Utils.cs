using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Api.Extensions
{
    public static class Utils
    {
        public static string DB_MYSQL
        {
            get
            {
                try
                {
                    var host = Environment.GetEnvironmentVariable("DB_MYSQL_HOST");
                    var user = Environment.GetEnvironmentVariable("DB_MYSQL_USER");
                    var pass = Environment.GetEnvironmentVariable("DB_MYSQL_PASS");
                    var name = Environment.GetEnvironmentVariable("DB_MYSQL_NAME");
                    var port = Environment.GetEnvironmentVariable("DB_MYSQL_PORT");

                    if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user) ||
                        string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(name) ||
                        string.IsNullOrWhiteSpace(port))
                    {
                        throw new Exception("Một hoặc nhiều biến môi trường cho database không được thiết lập.");
                    }

                    return $"Server={host};Port={port};Database={name};User={user};Password={pass};SslMode=None;AllowPublicKeyRetrieval=True;CharSet=utf8mb4;";
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Lỗi khi xây dựng chuỗi kết nối: {ex.Message}");
                    Console.ResetColor();
                    throw;
                }
            }
        }
    }
}