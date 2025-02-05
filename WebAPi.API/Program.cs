using WebAPI.API.Extension;
using WebAPI.API.Middleware;
using WebAPI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureSerilog();

builder.WebHost.ConfigureKestrelSettings();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMapperProfile();
builder.Services.AddRepositories();
builder.Services.ConfigureMediator();
builder.Services.AddCustomCors();

builder.Services.ConfigureSwagger();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddControllers();

var app = builder.Build();

app.MigrateDatabase();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseCustomSwagger();

app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.UseRouting();
app.MapControllers();

app.Run();
