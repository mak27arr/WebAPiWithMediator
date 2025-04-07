using Products.Common.API.Extension;
using Products.Common.API.Middleware;
using Products.Common.Auth.Extension;
using Products.Core.Extensions;
using Products.ProductAPI.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.ConfigureSerilog();

builder.WebHost.ConfigureKestrelSettings(builder.Configuration);
builder.Services.AddAuthConfig(builder.Configuration, builder.Environment);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCustomCors();
builder.Services.ConfigureApiVersion();
builder.Services.AddSwaggerServices(versions: "v1");
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddProductHealthChecks();
builder.Services.AddApplicationOpenTelemetry(builder.Environment);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ApplyCoreMaintenanceJobs();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCustomSwagger(versions: "v1");
app.ConfigureHttpsRedirection(builder.Configuration);

app.UseCors("AllowAnyOrigin");

app.UseRouting();
app.ConfigureAuthentication(app.Configuration);
app.UseHttpMetrics();
app.MapGrpcService<ProductGrpcService>();
app.MapMetrics();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();
