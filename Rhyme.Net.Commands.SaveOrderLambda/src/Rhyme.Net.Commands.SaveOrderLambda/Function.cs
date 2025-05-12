using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Rhyme.Net.Commands.SaveOrderLambda;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="request">The API gateway request object for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public APIGatewayHttpApiV2ProxyResponse FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        Console.WriteLine($"Lambda v48");

        context.Logger.LogLine($"Received request: {JsonSerializer.Serialize(request)}");

        // Read request body
        string paramKey = request.PathParameters.FirstOrDefault().Key ?? "No param provided";
        string paramValue = request.PathParameters.FirstOrDefault().Value ?? "No param provided";
        string body = request.Body ?? "No Body Provided";

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(new { message = $"Save Order Test!", body, paramKey, paramValue }),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
