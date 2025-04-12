using Rhyme.Net.Core.Domain.OrderAggregate;

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
}