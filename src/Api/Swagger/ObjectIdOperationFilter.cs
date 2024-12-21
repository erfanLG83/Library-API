using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace Api.Swagger;

public class stringOperationFilter : IOperationFilter
{
    private static readonly IEnumerable<string> stringIgnoreParameters =
    [
            "Timestamp",
            "Machine",
            "Pid",
            "Increment",
            "CreationTime"
    ];

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var p in operation.Parameters.ToList())
            if (stringIgnoreParameters.Any(x => p.Name.EndsWith(x)))
            {
                var parameterIndex = operation.Parameters.IndexOf(p);
                operation.Parameters.Remove(p);
                var dotIndex = p.Name.LastIndexOf(".");
                if (dotIndex > -1)
                {
                    var idName = p.Name.Substring(0, dotIndex);
                    if (!operation.Parameters.Any(x => x.Name == idName))
                    {
                        operation.Parameters.Insert(parameterIndex, new OpenApiParameter()
                        {
                            Name = idName,
                            Schema = new OpenApiSchema()
                            {
                                Type = "string",
                                Format = "24-digit hex string"
                            },
                            Description = GetFieldDescription(idName, context),
                            Example = new OpenApiString(string.Empty.ToString()),
                            In = p.In,
                        });
                    }
                }
            }
    }

    private string? GetFieldDescription(string idName, OperationFilterContext context)
    {
        var name = char.ToUpperInvariant(idName[0]) + idName.Substring(1);
        var classProp = context.MethodInfo.GetParameters().FirstOrDefault()?.ParameterType?.GetProperties().FirstOrDefault(x => x.Name == name);
        var typeAttr = classProp != null
            ? classProp.GetCustomAttribute<DescriptionAttribute>()!
            : null;

        return typeAttr?.Description;
    }
}