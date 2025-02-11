using Gateway.Extension;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddAuthConfig(builder.Configuration, builder.Environment);
builder.Services.ConfigureSwagger();
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.ConfigureAuthentication(builder.Configuration);
app.UseCustomSwagger();
await app.UseOcelot();

app.Run("http://0.0.0.0:8080");
