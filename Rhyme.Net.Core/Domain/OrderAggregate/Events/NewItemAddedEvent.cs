using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.OrderAggregate.Events;

public class NewOrderItemAddedEvent : DomainEventBase
{
  public Order Order { get; set; }
  public OrderItem NewItem { get; set; }

  public NewOrderItemAddedEvent(Order order, OrderItem newItem)
  {
    Order = order;
    NewItem = newItem;
  }
}
