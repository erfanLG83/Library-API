using Application.Books.Common.Models;
using Application.Common.Interfaces;
using Application.Common.Settings;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<DatabaseInitializer>();
        services.AddSingleton<IDateTimeProvider, Services.DateTimeProvider>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<ITokenFactoryService, TokenFactoryService>();
        services.AddScoped<ISmsService, TestSmsService>();
        services.AddSingleton<IFileManager, FileManager>();
        services.AddMemoryCache();
        services.AddSettings(configuration);

        services.AddElasticsearch(configuration);
    }

    private static IServiceCollection AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new ElasticsearchClientSettings(new Uri(configuration["ConnectionStrings:Elastic:Url"]!))
            .ServerCertificateValidationCallback((_, _, _, _) => true)
            .DisableDirectStreaming()
            .OnRequestCompleted(x =>
            {
                if (x.HasSuccessfulStatusCode)
                    return;

                Console.WriteLine(x.ToString());
                Debug.WriteLine(x.ToString());
            })
            .DefaultMappingFor<BookDto>(x => x.IndexName("books"))
            .Authentication(new BasicAuthentication(configuration["ConnectionStrings:Elastic:Username"]!, configuration["ConnectionStrings:Elastic:Password"]!));
        var client = new ElasticsearchClient(settings);

        return services.AddSingleton(client);
    }

    private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(_ =>
        {
            BearerTokenSettings bearerTokenSettings = new();
            configuration.GetRequiredSection("BearerTokenSettings").Bind(bearerTokenSettings);
            return bearerTokenSettings;
        });

        services.AddSingleton(_ =>
        {
            AdminData adminData = new();
            configuration.GetRequiredSection("AdminData").Bind(adminData);
            return adminData;
        });

        return services;
    }
}