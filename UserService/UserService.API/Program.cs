using Products.Common.API.Extension;
using Products.Common.API.Middleware;
using Products.Common.Auth.Extension;
using Prometheus;
using UserService.API.Extensions;
using UserService.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.ConfigureSerilog();

builder.WebHost.ConfigureKestrelSettings(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddCustomCors();
builder.Services.ConfigureApiVersion();
builder.Services.AddSwaggerServices(versions: "v1");

builder.Services.AddControllers();
builder.Services.AddAuthConfig(builder.Configuration, builder.Environment);
builder.Services.AddProductHealthChecks();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCustomSwagger(versions: "v1");

app.ConfigureHttpsRedirection(builder.Configuration);

app.UseCors("AllowAnyOrigin");

app.UseRouting();
app.UseHttpMetrics();
app.ConfigureAuthentication(app.Configuration);
app.MapMetrics();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();
