using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

public static class DynamoDbConfiguration
{
  public static IDynamoDBContext ConfigureDynamoDbContext()
  {
    var serviceUrl = Environment.GetEnvironmentVariable("DYNAMODB_SERVICE_URL");
    var accessKey = Environment.GetEnvironmentVariable("DYNAMODB_ACCESS_KEY");
    var secretKey = Environment.GetEnvironmentVariable("DYNAMODB_SECRET_KEY");
    var region = Environment.GetEnvironmentVariable("DYNAMODB_REGION");

    if (string.IsNullOrEmpty(serviceUrl) ||
        string.IsNullOrEmpty(accessKey) ||
        string.IsNullOrEmpty(secretKey) ||
        string.IsNullOrEmpty(region))
    {
      throw new InvalidOperationException("DynamoDB environment variables are not properly configured.");
    }

    var config = new AmazonDynamoDBConfig()
    {
      ServiceURL = serviceUrl,
      RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
    };

    var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
    var client = new AmazonDynamoDBClient(credentials, config);

    return new DynamoDBContext(client);
  }
}
