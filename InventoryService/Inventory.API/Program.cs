using Inventory.API.Extension;
using Inventory.Application.Extensions;
using Products.Common.API.Extension;
using Products.Common.API.Middleware;
using Products.Common.Auth.Extension;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.ConfigureSerilog();

builder.WebHost.ConfigureKestrelSettings(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCommunicationServices();
builder.Services.AddCustomCors();
builder.Services.ConfigureApiVersion();
builder.Services.AddSwaggerServices(versions: "v1");
builder.Services.AddAuthConfig(builder.Configuration, builder.Environment);
builder.Services.AddControllers();
builder.Services.AddProductHealthChecks();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ApplyApplicationMaintenanceJobs();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCustomSwagger(versions: "v1");
app.ConfigureHttpsRedirection(builder.Configuration);

app.UseCors("AllowAnyOrigin");

app.UseRouting();
app.ConfigureAuthentication(app.Configuration);
app.UseHttpMetrics();
app.MapMetrics();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();
