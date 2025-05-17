using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Rhyme.Net.Infrastructure.Data.NoSQL;

var services = new ServiceCollection();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IDynamoDBContext>(_ =>
    {
        var region = Environment.GetEnvironmentVariable("DYNAMODB_REGION") ?? "eu-central-1";
        var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName(region));
        return new DynamoDBContext(client);
    })
    .AddSingleton<OrderRepository>()
    .BuildServiceProvider();