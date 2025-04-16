using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Rhyme.Net.Core.Domain.OrderAggregate;
using Rhyme.Net.Core.Sourcing;

public static class SampleData
{

  public static IEnumerable<Order> GetTestOrders()
  {
    var order1 = new Order(Guid.NewGuid());
    order1.AddItem(new OrderItem("Item 1", "It's a nice one.", 10.0m));
    order1.AddItem(new OrderItem("Item 2", "It's a better one.", 20.0m));
    order1.AddItem(new OrderItem("Item 3", "It's the best one.", 30.0m));

    var order2 = new Order(Guid.NewGuid());
    order2.UpdateStatus();
    order2.AddItem(new OrderItem("Item 1", "It's a nice one.", 10.0m));

    var order3 = new Order(Guid.NewGuid());
    order3.UpdateStatus();
    order3.UpdateStatus();
    order3.AddItem(new OrderItem("Item 1", "It's a nice one.", 10.0m));
    order3.AddItem(new OrderItem("Item 2", "It's a better one.", 20.0m));

    return new List<Order>
    {
      order1,
      order2,
      order3
    };
  }

  public static IEnumerable<Event> GetTestEvents()
  {
    var testEvent1 = new Event();
    testEvent1.AggregateId = GetTestOrders().First().Id.ToString();
    testEvent1.AggregateName = Aggregate.Order.ToString();
    testEvent1.Name = EventName.OrderInitiated;
    testEvent1.Payload = JsonSerializer.Serialize(GetTestOrders().First());
    testEvent1.SequenceNumber = 1;
    testEvent1.Issuer = EventIssuer.Customer;

    var testEvent2 = new Event();
    testEvent2.AggregateId = GetTestOrders().Skip(1).First().Id.ToString();
    testEvent2.AggregateName = Aggregate.Order.ToString();
    testEvent2.Name = EventName.OrderItemAdded;
    testEvent2.Payload = JsonSerializer.Serialize(GetTestOrders().Skip(1).First());
    testEvent2.SequenceNumber = 2;
    testEvent2.Issuer = EventIssuer.POS;

    var testEvent3 = new Event();
    testEvent3.AggregateId = GetTestOrders().Last().Id.ToString();
    testEvent3.AggregateName = Aggregate.Order.ToString();
    testEvent3.Name = EventName.OrderPaymentMethodSelected;
    testEvent3.Payload = JsonSerializer.Serialize(GetTestOrders().Last());
    testEvent3.SequenceNumber = 3;
    testEvent3.Issuer = EventIssuer.Customer;

    return new List<Event>()
    {
      testEvent1,
      testEvent2,
      testEvent3
    };
  }
}