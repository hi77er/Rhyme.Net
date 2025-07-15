using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Rhyme.Net.Core.Domain.CouponAggregate;
using Rhyme.Net.Core.Interfaces;
using Rhyme.Net.Core.Services;
using Rhyme.Net.UseCases.Coupons;
using Rhyme.Net.UseCases.Coupons.GenerateForCampaign;

public class Program
{
  private static IServiceProvider? _serviceProvider;
  private static GenerateForCampaignHandler? _handler;

  static void Main(string[] args)
  {
    Console.WriteLine($"HANDLER: Lambda v61; Args[]: {string.Join(", ", args)};");

    var request = MapRequest(args);
    Console.WriteLine($"HANDLER: Received request: {JsonSerializer.Serialize(request)}");

    Guard.Against.Null(request, nameof(request));
    var command = new GenerateForCampaignCommand(request.CampaignId, request.TotalCouponsCount);

    _serviceProvider = ConfigureServices();
    _handler = _serviceProvider.GetRequiredService<GenerateForCampaignHandler>();

    var result = _handler.Handle(command, CancellationToken.None).Result;
    Console.WriteLine($"HANDLER: Success = {result.IsSuccess};");
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
        .AddSingleton<IDynamoRepository<Coupon, string>, DynamoRepository<Coupon, string>>()
        .AddSingleton<IGenerateCampaignCouponsService, GenerateCampaignCouponsService>()
        .AddSingleton<GenerateForCampaignHandler>()
        .BuildServiceProvider();

    return serviceProvider;
  }

  private static CouponsForCampaignRequestBody? MapRequest(string[] args)
  {
    if (args.Length < 2)
    {
      Console.WriteLine("Usage: <CampaignId> <TotalCouponsCount>");
      return null;
    }

    var campaignId = args[0];
    if (!int.TryParse(args[1], out var totalCouponsCount))
    {
      Console.WriteLine("Invalid TotalCouponsCount argument.");
      return null;
    }

    return new CouponsForCampaignRequestBody(campaignId, totalCouponsCount);
  }
}