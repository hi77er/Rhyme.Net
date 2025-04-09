using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.OrderAggregate.Events;

public class OrderCompleteEvent : DomainEventBase
{
  public Order Order { get; set; }

  public OrderCompleteEvent(Order order)
  {
    Order = order;
  }
}
