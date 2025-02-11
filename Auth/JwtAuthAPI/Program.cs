using JwtAuthAPI.Extension;
using JwtAuthManager.Extension;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.WebHost.ConfigureKestrelSettings(builder.Configuration);

builder.Services.AddJwtAuthManager(builder.Configuration);
builder.Services.ConfigureMediator();
builder.Services.AddControllers();

var app = builder.Build();

app.ConfigureHttpsRedirection(builder.Configuration);

app.UseRouting();
app.MapControllers();

app.Run();
