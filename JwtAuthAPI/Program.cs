using JwtAuthManager.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthManager();
builder.Services.ConfigureMediator();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();
