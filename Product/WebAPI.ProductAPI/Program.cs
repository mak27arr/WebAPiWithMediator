using Products.Common.API.Extension;
using Products.Common.API.Middleware;
using WebAPI.Core.Extensions;
using WebAPI.ProductAPI.Extension;
using WebAPI.ProductAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.ConfigureSerilog();

builder.WebHost.ConfigureKestrelSettings(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCustomCors();
builder.Services.ConfigureSwagger();

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddAuthConfig(builder.Configuration, builder.Environment);
builder.Services.AddProductHealthChecks();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ApplyCoreMaintenanceJobs();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCustomSwagger();

app.ConfigureHttpsRedirection(builder.Configuration);

app.UseCors("AllowAnyOrigin");

app.UseRouting();
app.ConfigureAuthentication(app.Configuration);
app.MapGrpcService<ProductGrpcService>();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();
