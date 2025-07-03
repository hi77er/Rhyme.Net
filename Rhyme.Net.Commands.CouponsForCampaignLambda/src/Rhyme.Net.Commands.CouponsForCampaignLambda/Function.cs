using System.Text.Json;
using Amazon.Batch;
using Amazon.Batch.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Rhyme.Net.UseCases.Coupons;
using Rhyme.Net.UseCases.Coupons.GenerateForCampaign;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Rhyme.Net.Commands.CouponsForCampaignLambda;

public class Function
{

    // private GenerateForCampaignHandler? _handler;

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="request">The API gateway request object for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        context.Logger.LogLine($"HANDLER: Lambda v58");
        context.Logger.LogLine($"HANDLER: Received request: {JsonSerializer.Serialize(request)}");

        var requestBody = JsonSerializer.Deserialize<CouponsForCampaignRequestBody>(request.Body);
        Guard.Against.Null(requestBody, nameof(requestBody));

        var batchClient = new AmazonBatchClient();
        var submitRequest = new SubmitJobRequest
        {
            JobName = "CouponsForCampaignJob",
            JobQueue = "your-job-queue-name",
            JobDefinition = "your-job-definition-name",
            ContainerOverrides = new ContainerOverrides
            {
                Command = new List<string> { requestBody.CampaignId, requestBody.TotalCouponsCount.ToString() }
            }
        };

        var jobResponse = await batchClient.SubmitJobAsync(submitRequest);


        context.Logger.LogLine($"HANDLER: Йоб респонсе: {JsonSerializer.Serialize(jobResponse)}");
        var response = new CouponsForCampaignResponse(jobResponse.HttpStatusCode == System.Net.HttpStatusCode.OK);
        return BuildResponse(response);
    }

    private static IServiceProvider ConfigureServices()
    {
        var serviceProvider = new ServiceCollection()
            // .AddSingleton<IAmazonDynamoDB>(_ =>
            // {
            //     var region = Environment.GetEnvironmentVariable("DYNAMODB_REGION") ?? "eu-central-1";
            //     var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName(region));
            //     return client;
            // })
            // .AddSingleton<IDynamoDBContext>(_ =>
            // {
            //     var region = Environment.GetEnvironmentVariable("DYNAMODB_REGION") ?? "eu-central-1";
            //     var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName(region));
            //     return new DynamoDBContext(client);
            // })
            // .AddSingleton<IDynamoRepository<Coupon, string>, DynamoRepository<Coupon, string>>()
            // .AddSingleton<IGenerateCampaignCouponsService, GenerateCampaignCouponsService>()
            // .AddSingleton<GenerateForCampaignHandler>()
            .BuildServiceProvider();

        return serviceProvider;
    }

    private APIGatewayHttpApiV2ProxyResponse BuildResponse(CouponsForCampaignResponse response)
    {
        return new APIGatewayHttpApiV2ProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(response.Success),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
