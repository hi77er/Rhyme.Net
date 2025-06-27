using Amazon.DynamoDBv2.DataModel;
using Rhyme.Net.Core.Converters;
using Rhyme.Net.Core.Interfaces;

namespace Rhyme.Net.Core.Sourcing;

[DynamoDBTable("events")]
public class Event : DynamoDbEntity, IEvent
{
  [DynamoDBHashKey("aggregateName")]
  public string AggregateName { get; set; } = string.Empty;

  [DynamoDBRangeKey("aggregateId")]
  public string AggregateId { get; set; } = string.Empty;

  [DynamoDBProperty("name", Converter = typeof(EventNameConverter))]
  public EventName? Name { get; set; }

  [DynamoDBProperty("sequenceNumber")]
  public int SequenceNumber { get; set; }

  [DynamoDBProperty("issuer", Converter = typeof(EventIssuerConverter))]
  public EventIssuer? Issuer { get; set; }

  [DynamoDBProperty("payload")]
  public string Payload { get; set; } = string.Empty;

  [DynamoDBProperty("issuedAt", Converter = typeof(DateTimeConverter))]
  public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

  [DynamoDBProperty("createdAt", Converter = typeof(DateTimeConverter))]
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}