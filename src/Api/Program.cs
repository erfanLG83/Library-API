using Api;
using Application;
using Application.Books.Common.Models;
using Application.Books.Queries.GetAll;
using Elastic.Clients.Elasticsearch;
using Infrastructure;
using Infrastructure.Persistence;
using MediatR;
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
    var elastic = app.ApplicationServices.GetRequiredService<ElasticsearchClient>();

    try
    {
        await InitializeDatabaseAsync(app);
        await InitializeElasticsearchAsync(logger, elastic);

        var countResponse = await elastic.CountAsync();
        if (countResponse.IsValidResponse == false)
            throw new Exception("Failed to get count of documents from elasticsearch");

        if (false) // feature flag - manual change
        {
            var mediator = app.ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<IMediator>();

            var books = await mediator.Send(new GetAllBooksQuery(null)
            {
                PageIndex = 1,
                PageSize = int.MaxValue
            });


            await elastic.IndexManyAsync(books.Items);
        }

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

static async Task InitializeElasticsearchAsync(ILogger<Program> logger, ElasticsearchClient elastic)
{
    var elasticResponse = await elastic.HealthReportAsync();
    if (elasticResponse.IsSuccess() == false)
    {
        logger.LogCritical("Elasticsearch healthcheck failed | {@ElasticResponse}", elasticResponse);
        throw new Exception("Elasticsearch healthcheck failed");
    }

    await elastic.Indices.PutMappingAsync<BookDto>(mappings => mappings
        .Indices("books")
        .Properties(properties => properties
            .Nested(x => x.BookInBranches)
        ));
}

public partial class Program { }