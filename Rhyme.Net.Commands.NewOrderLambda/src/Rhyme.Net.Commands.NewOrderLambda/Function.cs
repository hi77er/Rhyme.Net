using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Serialization.SystemTextJson;
using System.Text.Json;
using Rhyme.Net.Infrastructure.Data.NoSQL;
using Microsoft.Extensions.DependencyInjection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Rhyme.Net.Commands.NewOrderLambda;

public class Function
{
    private IServiceProvider? _serviceProvider;
    private OrderRepository? _orderRepository;

    // public Function()
    // {
    //     _serviceProvider = ConfigureServices();
    //     _orderRepository = _serviceProvider.GetRequiredService<OrderRepository>();
    // }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="request">The API gateway request object for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public APIGatewayHttpApiV2ProxyResponse FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        context.Logger.LogLine($"HANDLER: Lambda v49");
        context.Logger.LogLine($"HANDLER: Received request: {JsonSerializer.Serialize(request)}");

        _serviceProvider = ConfigureServices();
        _orderRepository = _serviceProvider.GetRequiredService<OrderRepository>();
        var allOrders = _orderRepository.GetAllAsync().Result; // Call the repository method
        context.Logger.LogLine($"HANDLER: {allOrders.Count()} orders found.");

        // Read request body
        string requestBody = request.Body ?? "No Body Provided";

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(new { message = $"Order ID: test", body = requestBody }),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private static IServiceProvider ConfigureServices()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IDynamoDBContext>(_ =>
            {
                var region = Environment.GetEnvironmentVariable("DYNAMODB_REGION") ?? "eu-central-1";
                var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName(region));
                return new DynamoDBContext(client);
            })
            .AddSingleton<OrderRepository>()
            .BuildServiceProvider();

        return serviceProvider;
    }
}
