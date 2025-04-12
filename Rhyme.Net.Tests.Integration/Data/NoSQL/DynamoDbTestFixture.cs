using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.OrderAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Integration.Data.NoSQL;

public class DynamoDbTestFixture<TDomainAggregate> where TDomainAggregate : class, IAggregateRoot
{
  private readonly IDynamoDBContext _dbContext;

  public DynamoDbTestFixture()
  {
    Environment.SetEnvironmentVariable("DYNAMODB_SERVICE_URL", "http://localhost:8000"); // Local DynamoDB
    Environment.SetEnvironmentVariable("DYNAMODB_ACCESS_KEY", "test-access-key");
    Environment.SetEnvironmentVariable("DYNAMODB_SECRET_KEY", "test-secret-key");
    Environment.SetEnvironmentVariable("AWS_REGION", "eu-central-1");

    // Configure the DynamoDB context for a local/test DynamoDB instance
    var client = new AmazonDynamoDBClient();
    _dbContext = new DynamoDBContext(client);
  }


  protected DynamoRepository<Order> GetRepository()
  {
    return new DynamoRepository<Order>(_dbContext);
  }

}