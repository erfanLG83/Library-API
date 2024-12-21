using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Swagger;

public class stringSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(string))
        {
            schema.Type = "string";
            schema.Format = "24-digit hex string";
            schema.Example = new OpenApiString(string.Empty.ToString());
        }
    }
}