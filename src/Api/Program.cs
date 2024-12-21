using Api;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureService(builder.Configuration, builder.Environment);
builder.Services.AddApiServices(builder.Configuration);

if (builder.Environment.IsEnvironment("Test") == false)
{
    builder.Logging.ClearProviders();

    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.Enrich.FromLogContext();
        configuration.ReadFrom.Configuration(context.Configuration);
    });
}

var app = builder.Build();

await StartupTasksAsync(app);

// Configure the HTTP request pipeline.

if (app.Environment.IsProduction() == false)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
        options.DocExpansion(DocExpansion.None);
    });
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();

if (app.Environment.IsProduction())
    app.UseCors("CorsPolicy");
else
    app.UseCors("DevelopmentCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(options => { });

app.MapControllers();

app.Run();


static async Task StartupTasksAsync(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
    try
    {
        await InitializeDatabaseAsync(app);
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "failed to complete startup tasks | {message}", ex.Message);
        throw;
    }
}

static async Task InitializeDatabaseAsync(IApplicationBuilder app)
{
    await using var scope = app.ApplicationServices.CreateAsyncScope();

    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();

    await initializer.SeedDataAsync();
}

public partial class Program { }
