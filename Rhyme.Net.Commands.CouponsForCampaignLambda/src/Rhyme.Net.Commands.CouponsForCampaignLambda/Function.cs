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
    private IBatchJobScheduler? _scheduler;

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
        var args = new List<string> { requestBody.CampaignId, requestBody.TotalCouponsCount.ToString() };

        var provider = ConfigureServices();
        Guard.Against.Null(provider, nameof(provider));

        _scheduler = provider.GetRequiredService<IBatchJobScheduler>();

        var jobId = await _scheduler.ScheduleAsync(
            jobName: "GenerateCouponsForCampaign",
            jobQueue: "coupon-generation-job-queue",
            jobDefinition: "CouponsForCampaignJob-dev-def",
            args: args);

        context.Logger.LogLine($"HANDLER: JobId: {jobId}");
        var response = new CouponsForCampaignResponse(jobId);
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
            .AddSingleton<IAmazonBatch, AmazonBatchClient>()
            .AddSingleton<IBatchJobScheduler, BatchJobScheduler>()
            .BuildServiceProvider();

        return serviceProvider;
    }

    private APIGatewayHttpApiV2ProxyResponse BuildResponse(CouponsForCampaignResponse response)
    {
        return new APIGatewayHttpApiV2ProxyResponse()
        {
            StatusCode = 200,
            Body = $"JobId: {response.JobId}",
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
