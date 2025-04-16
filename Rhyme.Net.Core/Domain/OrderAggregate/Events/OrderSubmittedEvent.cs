using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.OrderAggregate.Events;

public class OrderSubmittedEvent : DomainEventBase
{
  public Order Order { get; set; }

  public OrderSubmittedEvent(Order order)
  {
    Order = order;
  }
}
