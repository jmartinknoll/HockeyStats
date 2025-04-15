using MediatR;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.NHLClient.Endpoints;
using Domain.Interfaces;
using System.Reflection;
using HockeyStatsApp;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Ensure the correct endpoint path for Swagger
    options.SwaggerDoc("v1", new() { Title = "Hockey Stats API", Version = "v1" });
});

builder.Services.AddScoped<INhlClient, NhlClient>();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<HockeyStatsApp.HockeyStatsApp>());

// Register HttpClient
builder.Services.AddHttpClient<NhlClient>(client =>
{
    client.BaseAddress = new Uri("https://api-web.nhle.com/v1");
});

var app = builder.Build();

// Use routing middleware before swagger middleware
app.UseRouting();

// Ensure Swagger is enabled in the development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Ensure Swagger UI is available at the correct URL
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hockey Stats API v1");
    });
}

// Use controllers after Swagger setup
app.MapControllers();

app.Run();
