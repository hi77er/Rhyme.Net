using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Rhyme.Net.Handlers.OrderEventsHandlerLambda;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="dynamoDbEvent">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public void FunctionHandler(DynamoDBEvent dynamoDbEvent, ILambdaContext context)
    {
        Console.WriteLine($"Lambda v49");
        Console.WriteLine($"Processing {dynamoDbEvent.Records.Count} records...");

        foreach (var record in dynamoDbEvent.Records)
        {
            if (record.EventName == "INSERT") // Check if it's a create event
            {
                Console.WriteLine($"New item added with Event ID: {record.EventID}");

            }
        }

        Console.WriteLine("Stream processing complete.");
    }
}
