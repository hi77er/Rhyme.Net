using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Rhyme.Net.Core.Domain.CouponAggregate;

namespace Rhyme.Net.Infrastructure.Data.NoSQL;

public class CouponRepository : DynamoRepository<Coupon, string>
{
  private const int BatchSize = 25;

  public CouponRepository(IAmazonDynamoDB dbClient, IDynamoDBContext dbContext) : base(dbClient, dbContext)
  {
  }

  public async Task WriteCouponsBatchAsync(IEnumerable<Coupon> coupons)
  {
    var batch = new List<WriteRequest>();

    foreach (var coupon in coupons)
    {
      var item = new Dictionary<string, AttributeValue>
      {
        ["id"] = new AttributeValue { S = coupon.Id },
        ["campaignId"] = new AttributeValue { S = coupon.CampaignId }
      };

      batch.Add(new WriteRequest
      {
        PutRequest = new PutRequest { Item = item }
      });

      if (batch.Count == BatchSize)
      {
        await FlushBatchAsync(batch);
        batch.Clear();
      }
    }

    if (batch.Count > 0)
      await FlushBatchAsync(batch);
  }

  private async Task FlushBatchAsync(List<WriteRequest> batch)
  {
    var request = new BatchWriteItemRequest
    {
      RequestItems = new Dictionary<string, List<WriteRequest>>
      {
        [_tableName] = batch
      }
    };

    var response = await _dbClient.BatchWriteItemAsync(request);

    if (response.UnprocessedItems.Count > 0)
    {
      // Simple retry (can be expanded with exponential backoff)
      await Task.Delay(250);
      await FlushBatchAsync(response.UnprocessedItems[_tableName]);
    }
  }

}