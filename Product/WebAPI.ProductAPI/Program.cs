using WebAPI.ProductAPI.Extension;
using WebAPI.ProductAPI.Middleware;
using WebAPI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.ConfigureSerilog();

builder.WebHost.ConfigureKestrelSettings(builder.Configuration);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMapperProfile();
builder.Services.AddRepositories();
builder.Services.ConfigureMediator();
builder.Services.AddCustomCors();

builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddProductHealthChecks();

var app = builder.Build();
app.MigrateDatabase();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCustomSwagger();

app.ConfigureHttpsRedirection(builder.Configuration);

app.UseCors("AllowAnyOrigin");

app.UseRouting();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();
