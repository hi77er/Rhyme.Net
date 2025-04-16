using Ardalis.GuardClauses;
using Rhyme.Net.Core.Domain.OrderAggregate;

namespace Rhyme.Net.Core.Sourcing.Payloads;

public class OrderItemAddedPayload
{
  public OrderItem? Item { get; set; }

  public OrderItemAddedPayload() { }
  public OrderItemAddedPayload(OrderItem item)
  {
    Guard.Against.Null(item, "item");
    Item = item;
  }
}
