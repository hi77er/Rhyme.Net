using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Microsoft.Extensions.DependencyInjection;
using Rhyme.Net.Infrastructure.Data.NoSQL;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Rhyme.Net.Handlers.OrderEventsHandlerLambda;

public class Function
{
    private readonly IServiceProvider _serviceProvider;
    private readonly OrderRepository _orderRepository;

    public Function()
    {
        _serviceProvider = ConfigureServices();
        _orderRepository = _serviceProvider.GetRequiredService<OrderRepository>();
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="dynamoDbEvent">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public void FunctionHandler(DynamoDBEvent dynamoDbEvent, ILambdaContext context)
    {
        Console.WriteLine($"HANDLER: Lambda v49");
        Console.WriteLine($"HANDLER: Processing {dynamoDbEvent.Records.Count} records...");

        foreach (var record in dynamoDbEvent.Records)
        {
            if (record.EventName == "INSERT") // Check if it's a create event
            {
                Console.WriteLine($"HANDLER: New item added with Event ID: {record.EventID}");

                var allOrders = _orderRepository.GetAllAsync().Result; // Call the repository method

                Console.WriteLine($"HANDLER: {allOrders.Count()} orders found.");
            }
        }

        Console.WriteLine("HANDLER: Stream processing complete.");
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
