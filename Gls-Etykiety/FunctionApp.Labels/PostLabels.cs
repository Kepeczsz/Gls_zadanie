using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Gls_Etykiety.azure_functions;

public class PostLabels
{
    private readonly ILogger _logger;

    public PostLabels(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<PostLabels>();
    }

    [Function("PostLabels")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Welcome to Azure Functions!");

        return response;
    }
}
