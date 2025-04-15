using Amazon.DynamoDBv2.DataModel;
using Rhyme.Net.Core.Converters;

[DynamoDBTable("events")]
public class Events
{
  [DynamoDBHashKey("id")]
  [DynamoDBProperty(Converter = typeof(GuidConverter))]
  public Guid Id { get; set; }

  [DynamoDBProperty("sequenceNumber")]
  public int SequenceNumber { get; set; }

  [DynamoDBProperty("name")]
  public string Name { get; set; } = string.Empty;

  [DynamoDBProperty("stream", Converter = typeof(EventStreamConverter))]
  public EventStream? Stream { get; set; }

  [DynamoDBProperty("aggregateName")]
  public string AggregateName { get; set; } = string.Empty;

  [DynamoDBProperty("aggregateId")]
  public string AggregateId { get; set; } = string.Empty;

  [DynamoDBProperty("issuer", Converter = typeof(EventIssuerConverter))]
  public EventIssuer? Issuer { get; set; }

  [DynamoDBProperty("payload")]
  public string Payload { get; set; } = string.Empty;

  [DynamoDBProperty("issuedAt", Converter = typeof(DateTimeConverter))]
  public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

  [DynamoDBProperty("createdAt", Converter = typeof(DateTimeConverter))]
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}