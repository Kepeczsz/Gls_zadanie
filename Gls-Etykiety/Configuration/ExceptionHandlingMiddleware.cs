using Gls_Etykiety.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Gls_Etykiety.Configuration;

public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(NoDataFoundException ex)
        {
            await HandleErrorResponse(context, ex, HttpStatusCode.NotFound);  
        }

        catch (InvalidJsonBodyRequestException ex)
        {
            await HandleErrorResponse(context, ex, HttpStatusCode.BadRequest);
        }
        catch(GlsApiException ex)
        {
            await HandleErrorResponse(context, ex, HttpStatusCode.ServiceUnavailable);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleErrorResponse(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    public async Task HandleErrorResponse(FunctionContext context, Exception ex, HttpStatusCode statusCode)
    {
        var httpReqData = await context.GetHttpRequestDataAsync();

        if (httpReqData != null)
        {
            var newHttpResponse = httpReqData.CreateResponse(statusCode);

            await newHttpResponse.WriteAsJsonAsync(new { ErrorMessage = ex.Message, StatusCode = (int)statusCode });

            var invocationResult = context.GetInvocationResult();

            var httpOutputBindingFromMultipleOutputBindings = GetHttpOutputBindingFromMultipleOutputBinding(context);
            if (httpOutputBindingFromMultipleOutputBindings is not null)
            {
                httpOutputBindingFromMultipleOutputBindings.Value = newHttpResponse;
            }
            else
            {
                invocationResult.Value = newHttpResponse;
            }
        }
    }

    private OutputBindingData<HttpResponseData> GetHttpOutputBindingFromMultipleOutputBinding(FunctionContext context)
    {
        var httpOutputBinding = context.GetOutputBindings<HttpResponseData>()
            .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");

        return httpOutputBinding;
    }

}
