using Amazon.DynamoDBv2.DataModel;

namespace Rhyme.Net.Core.Sourcing;

[DynamoDBTable("checkpoints")]
public class Checkpoints
{

  // Value should follow the follwoing format: aggregateName-aggregateId
  [DynamoDBHashKey("key")]
  public string Key { get; set; } = string.Empty;

  [DynamoDBProperty("snapshot")]
  public string Snapshot { get; set; } = string.Empty;

  [DynamoDBProperty("lastEventSequenceNumber")]
  public int lastEventSequenceNumber { get; set; }


  [DynamoDBProperty("issuedAt", Converter = typeof(DateTimeConverter))]
  public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

  [DynamoDBProperty("createdAt", Converter = typeof(DateTimeConverter))]
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}