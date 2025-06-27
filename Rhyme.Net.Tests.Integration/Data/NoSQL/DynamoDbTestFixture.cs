using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.OrderAggregate;
using Rhyme.Net.Core.Sourcing;
using Rhyme.Net.Infrastructure.Data.NoSQL;
using Rhyme.Net.Infrastructure.Secrets;

namespace Rhyme.Net.Tests.Integration.Data.NoSQL;

public class DynamoDbTestFixture<TDomainAggregate> where TDomainAggregate : class, IAggregateRoot
{
  private readonly IAmazonDynamoDB _dbClient;
  private readonly IDynamoDBContext _dbContext;
  private readonly SecretsRepository _secretsRepository;

  public DynamoDbTestFixture()
  {
    Environment.SetEnvironmentVariable("DYNAMODB_ACCESS_KEY", "AKIAXYSEBQYL3WVJNVWI");
    Environment.SetEnvironmentVariable("DYNAMODB_SECRET_KEY_SECRET_NAME", "DYNAMODB_SECRET_KEY");
    Environment.SetEnvironmentVariable("AWS_REGION", "eu-central-1");

    var regionEndpoint = Amazon.RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_REGION"));
    _secretsRepository = new SecretsRepository(regionEndpoint);

    _dbClient = BuildClient();
    // Configure the DynamoDB context for a local/test DynamoDB instance
    _dbContext = new DynamoDBContext(_dbClient);
  }

  private AmazonDynamoDBClient BuildClient()
  {
    // Read environment variables
    var accessKey = Environment.GetEnvironmentVariable("DYNAMODB_ACCESS_KEY");
    var secretKeySecretName = Environment.GetEnvironmentVariable("DYNAMODB_SECRET_KEY_SECRET_NAME") ?? string.Empty;
    var region = Environment.GetEnvironmentVariable("AWS_REGION");

    var secretKey = _secretsRepository.GetSecretAsync(secretKeySecretName).Result;

    // Validate environment variables
    if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(region))
    {
      throw new InvalidOperationException("DynamoDB environment variables are not properly configured.");
    }

    // Configure the DynamoDB client
    var credentials = new BasicAWSCredentials(accessKey, secretKey);
    var config = new AmazonDynamoDBConfig()
    {
      RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region) // Example: "us-west-2"
    };

    return new AmazonDynamoDBClient(credentials, config);
  }

  protected DynamoRepository<Order, Guid> GetOrdersRepository()
  {
    return new DynamoRepository<Order, Guid>(_dbClient, _dbContext);
  }

  protected DynamoRepository<Event, Guid> GetEventsRepository()
  {
    return new DynamoRepository<Event, Guid>(_dbClient, _dbContext);
  }

  protected CouponRepository GetCouponsRepository()
  {
    return new CouponRepository(_dbClient, _dbContext);
  }
}