using System.Text;

namespace Web.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Log the request
        var request = await FormatRequest(context.Request);
        _logger.LogInformation("Request:\n{Request}", request);

        // Capture the response
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        // Log the response
        var response = await FormatResponse(context.Response);
        _logger.LogInformation("Response:\n{Response}", response);

        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
    }

    private static async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();
        var body = await new StreamReader(request.Body, Encoding.UTF8).ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);

        return $"{request.Method} {request.Path}{request.QueryString}\n{body}";
    }

    private static async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(response.Body, Encoding.UTF8).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return $"{response.StatusCode}\n{body}";
    }
}
