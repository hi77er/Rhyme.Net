using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Converters;

namespace Rhyme.Net.Core.Domain.CouponAggregate;

[DynamoDBTable("coupons")]
public class Coupon : DynamoDbEntity, IAggregateRoot
{
  [DynamoDBHashKey("id")]
  [DynamoDBProperty(Converter = typeof(GuidConverter))]
  public string Id { get; set; } = string.Empty;

  [DynamoDBRangeKey("campaignId")]
  [DynamoDBGlobalSecondaryIndexRangeKey("campaignId-index")]
  public string CampaignId { get; set; } = string.Empty;

  /// <summary>
  /// Default constructor for DynamoDB
  /// </summary>
  public Coupon() { }

  public Coupon(string newCouponId, string campaignId)
  {
    Id = Guard.Against.NullOrEmpty(newCouponId, nameof(newCouponId));
    CampaignId = Guard.Against.NullOrEmpty(campaignId, nameof(campaignId));
  }

  public override Dictionary<string, AttributeValue> ToAttributeValues()
  {
    return new Dictionary<string, AttributeValue>
    {
      ["id"] = new AttributeValue { S = Id },
      ["campaignId"] = new AttributeValue { S = CampaignId }
    };
  }
}
