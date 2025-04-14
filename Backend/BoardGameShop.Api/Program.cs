using BoardGameShop.Api.Data;
using BoardGameShop.Api.Repositories;
using BoardGameShop.Api.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using BoardGameShop.Api.Middleware;
using DotNetEnv;
using BoardGameShop.Api.Extensions;
using AutoMapper;
using BoardGameShop.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Load .env file
try
{
    var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
    Console.WriteLine($"Đường dẫn file .env: {envPath}");
    if (!File.Exists(envPath))
    {
        throw new FileNotFoundException("File .env không tồn tại.");
    }
    Env.Load(envPath);
    Console.WriteLine("File .env đã được nạp thành công.");
}
catch (Exception ex)
{
    Console.WriteLine($"Lỗi khi nạp file .env: {ex.Message}");
    throw;
}

// Kiểm tra biến môi trường (bao gồm cả JWT và Cloudinary)
var dbHost = Environment.GetEnvironmentVariable("DB_MYSQL_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_MYSQL_PORT");
var dbUser = Environment.GetEnvironmentVariable("DB_MYSQL_USER");
var dbPass = Environment.GetEnvironmentVariable("DB_MYSQL_PASS");
var dbName = Environment.GetEnvironmentVariable("DB_MYSQL_NAME");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var cloudinaryCloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
var cloudinaryApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
var cloudinaryApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");

if (string.IsNullOrWhiteSpace(dbHost)) throw new Exception("DB_MYSQL_HOST không được thiết lập.");
if (string.IsNullOrWhiteSpace(dbPort)) throw new Exception("DB_MYSQL_PORT không được thiết lập.");
if (string.IsNullOrWhiteSpace(dbUser)) throw new Exception("DB_MYSQL_USER không được thiết lập.");
if (string.IsNullOrWhiteSpace(dbPass)) throw new Exception("DB_MYSQL_PASS không được thiết lập.");
if (string.IsNullOrWhiteSpace(dbName)) throw new Exception("DB_MYSQL_NAME không được thiết lập.");
if (string.IsNullOrWhiteSpace(jwtIssuer)) throw new Exception("JWT_ISSUER không được thiết lập.");
if (string.IsNullOrWhiteSpace(jwtAudience)) throw new Exception("JWT_AUDIENCE không được thiết lập.");
if (string.IsNullOrWhiteSpace(jwtKey)) throw new Exception("JWT_KEY không được thiết lập.");
if (string.IsNullOrWhiteSpace(cloudinaryCloudName)) throw new Exception("CLOUDINARY_CLOUD_NAME không được thiết lập.");
if (string.IsNullOrWhiteSpace(cloudinaryApiKey)) throw new Exception("CLOUDINARY_API_KEY không được thiết lập.");
if (string.IsNullOrWhiteSpace(cloudinaryApiSecret)) throw new Exception("CLOUDINARY_API_SECRET không được thiết lập.");

Console.WriteLine($"DB_HOST: {dbHost}, DB_PORT: {dbPort}, DB_USER: {dbUser}, DB_NAME: {dbName}");
Console.WriteLine($"JWT_ISSUER: {jwtIssuer}, JWT_AUDIENCE: {jwtAudience}");
Console.WriteLine($"CLOUDINARY_CLOUD_NAME: {cloudinaryCloudName}");

// Logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

// Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BoardGameShop API",
        Version = "v1",
        Description = "API for BoardGameShop - A board game e-commerce platform"
    });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter token with Bearer prefix (e.g., Bearer {token})",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// EF Core with MariaDB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(Utils.DB_MYSQL, ServerVersion.AutoDetect(Utils.DB_MYSQL))
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors());

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Dependency Injection
builder.Services.AddScoped<IUserService, UserRepository>();


// Cấu hình Cloudinary
builder.Services.Configure<CloudinarySettings>(options =>
{
    options.CloudName = cloudinaryCloudName;
    options.ApiKey = cloudinaryApiKey;
    options.ApiSecret = cloudinaryApiSecret;
});

var app = builder.Build();

// Kiểm tra kết nối database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        bool canConnect = await dbContext.Database.CanConnectAsync();
        if (canConnect)
        {
            Console.WriteLine("Kết nối database thành công");
        }
        else
        {
            Console.WriteLine("Kết nối database thất bại: Không thể kết nối tới MariaDB.");
            throw new Exception("Không thể kết nối tới MariaDB.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Lỗi kết nối database: {ex.Message}");
        Console.WriteLine($"Chi tiết: {ex.StackTrace}");
        throw;
    }
}

// Middleware pipeline
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorizationMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BoardGameShop API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

app.Run();