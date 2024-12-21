using Api.Authorization;
using Api.Middlewares;
using Api.Services;
using Api.Swagger;
using Application.Common.Interfaces;
using Application.Common.Settings;
using Asp.Versioning;
using Domain.Entities.UserAggregate.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api;

public static class ConfigureServices
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSwaggerConfigurations();
        services.ConfigureAuthentication(configuration);
        services.AddControllers();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddCors(options =>
        {
            options.AddPolicy("DevelopmentCorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition")
                        .SetIsOriginAllowed(origin => true) // allow any origin
                        .AllowCredentials();
                });

            options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Content-Disposition")
                        .AllowCredentials()
                        .WithOrigins(configuration.GetSection("CorsOrigins").Get<string[]>() ?? []);
                });
        });

    }

    private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            BearerTokenSettings bearerTokenSettings = new();
            configuration.GetSection("BearerTokenSettings").Bind(bearerTokenSettings);

            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = bearerTokenSettings.Issuer,
                ValidateIssuer = true,
                ValidAudience = bearerTokenSettings.Audience,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bearerTokenSettings.SecretKey)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorizationBuilder()
            .AddPolicy(AppAuthorizationPolicies.SuperAdminPolicy, builder => builder.RequireRole(UserRole.SuperAdmin.ToString()));
    }
}