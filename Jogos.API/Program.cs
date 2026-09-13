using Azure.Monitor.OpenTelemetry.AspNetCore;
using Jogos.API.Application.Interfaces;
using Jogos.API.Application.UseCases;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Infrastructure.Data;
using Jogos.API.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options => {
    options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
});

builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddTransient<ICategoriaUseCase, CategoriaUseCase>();
builder.Services.AddTransient<IDesenvolvedoraRepository, DesenvolvedoraRepository>();
builder.Services.AddTransient<IDesenvolvedoraUseCase, DesenvolvedoraUseCase>();
builder.Services.AddTransient<IJogoRepository, JogoRepository>();
builder.Services.AddTransient<IJogoUseCase, JogoUseCase>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.EnableAnnotations();
});

// Adicionando Compactação
builder.Services.AddResponseCompression(options => {
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options => {
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options => {
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

// Adicionando Rate Limiter
builder.Services.AddRateLimiter(options => {

    options.AddFixedWindowLimiter(policyName: "politica_5_tentativas", opt => {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(20);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// Adicionando Health Checks
builder.Services.AddHealthChecks()
    // Liveness
    .AddCheck(
        "self",
        () => HealthCheckResult.Healthy(),
        tags: ["live"])
    // Readiness
    .AddOracle(
        connectionString: builder.Configuration.GetConnectionString("Oracle") ?? "",
        name: "oracle",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["db"]);

// Adicionando Application Insights (somente se a connection string estiver configurada)
var appInsightsConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
{
    builder.Services.AddOpenTelemetry()
        .UseAzureMonitor(options => {
            options.ConnectionString = appInsightsConnectionString;
        });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();
app.UseResponseCompression();

app.MapControllers();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

app.MapHealthChecks("/health/db", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db")
});

app.Run();
