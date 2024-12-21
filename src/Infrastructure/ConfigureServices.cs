using Application.Common.Interfaces;
using Application.Common.Settings;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<DatabaseInitializer>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<ITokenFactoryService, TokenFactoryService>();
        services.AddScoped<ISmsService, TestSmsService>();
        services.AddSingleton<IFileManager, FileManager>();
        services.AddMemoryCache();

        services.AddSettings(configuration);
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