using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class DynamoDbConfiguration
{
  public static IDynamoDBContext ConfigureDynamoDbContext()
  {
    var serviceUrl = Environment.GetEnvironmentVariable("DynamoDbServiceUrl");
    var accessKey = Environment.GetEnvironmentVariable("DynamoDbAccessKey");
    var secretKey = Environment.GetEnvironmentVariable("DynamoDbSecretKey");
    var region = Environment.GetEnvironmentVariable("DynamoDbRegion");

    if (string.IsNullOrEmpty(serviceUrl) || string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(region))
    {
      throw new InvalidOperationException("DynamoDB environment variables are not properly configured.");
    }

    var config = new AmazonDynamoDBConfig
    {
      ServiceURL = serviceUrl,
      RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
    };

    var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
    var client = new AmazonDynamoDBClient(credentials, config);

    return new DynamoDBContext(client);
  }
}
