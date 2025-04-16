using Ardalis.GuardClauses;
using Rhyme.Net.Core.Domain.OrderAggregate;

namespace Rhyme.Net.Core.Sourcing.Payloads;

public class OrderItemRemovedPayload
{
  public Guid? ItemId { get; set; }

  public OrderItemRemovedPayload() { }
  public OrderItemRemovedPayload(Guid itemId)
  {
    Guard.Against.Default(itemId, nameof(itemId));
    ItemId = itemId;
  }
}
