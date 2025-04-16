using Rhyme.Net.Core.Sourcing;

namespace Rhyme.Net.Core.Interfaces;

public interface IEvent
{
  string AggregateName { get; set; }
  string AggregateId { get; set; }
  int SequenceNumber { get; set; }
  EventName? Name { get; set; }
  EventIssuer? Issuer { get; set; }
  string Payload { get; set; }
  DateTime IssuedAt { get; set; }
  DateTime CreatedAt { get; set; }
}
