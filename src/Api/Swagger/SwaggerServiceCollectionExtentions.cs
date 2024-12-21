using Microsoft.OpenApi.Models;

namespace Api.Swagger;

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SchemaFilter<stringSchemaFilter>();

            options.OperationFilter<stringOperationFilter>();
            options.DocumentFilter<SwaggerAddEnumDescriptions>();
            options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
            options.CustomSchemaIds(SchemaIdSelector);
        });

        return services;

        static string SchemaIdSelector(Type modelType)
        {
            string? parentSchemaId = null;

            if (modelType.IsNested)
            {
                parentSchemaId = DefaultSchemaIdSelector(modelType.DeclaringType!);
            }

            return parentSchemaId != null ? $"{parentSchemaId}.{DefaultSchemaIdSelector(modelType)}" : DefaultSchemaIdSelector(modelType);
        }

        static string DefaultSchemaIdSelector(Type modelType)
        {
            if (!modelType.IsConstructedGenericType) return modelType.Name.Replace("[]", "Array");

            var prefix = modelType.GetGenericArguments()
                .Select(DefaultSchemaIdSelector)
                .Aggregate((previous, current) => previous + current);

            return prefix + modelType.Name.Split('`').First();
        }

    }
}