using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Serialization.SystemTextJson;
using System.Text.Json;
using Rhyme.Net.Infrastructure.Data.NoSQL;
using Microsoft.Extensions.DependencyInjection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Rhyme.Net.UseCases.Menus.Create;
using Rhyme.Net.UseCases.Orders.Create;
using Rhyme.Net.UseCases.Orders;
using Ardalis.GuardClauses;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Rhyme.Net.Commands.NewOrderLambda;

public class Function
{
    private IServiceProvider? _serviceProvider;
    private CreateOrderHandler? _handler;

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

        var requestBody = JsonSerializer.Deserialize<NewOrderRequestBody>(request.Body);
        Guard.Against.Null(requestBody, nameof(requestBody));

        var command = new CreateOrderCommand(requestBody.StoreId);

        _serviceProvider = ConfigureServices();
        _handler = _serviceProvider.GetRequiredService<CreateOrderHandler>();
        var newOrderId = _handler.Handle(command, CancellationToken.None).Result; // Call the repository method
        context.Logger.LogLine($"HANDLER: newOrderId = {newOrderId};");

        var response = new NewOrderResponse(newOrderId);

        return BuildResponse(response);
    }

    private static IServiceProvider ConfigureServices()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IAmazonDynamoDB>(_ =>
            {
                var region = Environment.GetEnvironmentVariable("DYNAMODB_REGION") ?? "eu-central-1";
                var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName(region));
                return client;
            })
            .AddSingleton<IDynamoDBContext>(_ =>
            {
                var region = Environment.GetEnvironmentVariable("DYNAMODB_REGION") ?? "eu-central-1";
                var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName(region));
                return new DynamoDBContext(client);
            })
            .AddSingleton<OrderRepository>()
            .AddSingleton<CreateOrderHandler>()
            .BuildServiceProvider();

        return serviceProvider;
    }

    private APIGatewayHttpApiV2ProxyResponse BuildResponse(NewOrderResponse response)
    {
        return new APIGatewayHttpApiV2ProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(response),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
