using OrderService.API.Extension;
using OrderService.Application.Extensions;
using Products.Common.API.Extension;
using Products.Common.API.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.ConfigureSerilog();

builder.WebHost.ConfigureKestrelSettings(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCustomCors();

builder.Services.ConfigureSwagger();
builder.Services.AddControllers();
builder.Services.AddAuthConfig(builder.Configuration, builder.Environment);
builder.Services.AddProductHealthChecks();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ApplyApplicationMaintenanceJobs();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCustomSwagger();

app.ConfigureHttpsRedirection(builder.Configuration);

app.UseCors("AllowAnyOrigin");

app.UseRouting();
app.ConfigureAuthentication(app.Configuration);
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();