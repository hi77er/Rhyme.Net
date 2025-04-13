using Amazon.DynamoDBv2.DataModel;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.OrderAggregate.Events;

namespace Rhyme.Net.Core.Domain.OrderAggregate;

[DynamoDBTable("Orders")]
public class Order : HasDomainEventsBase, IAggregateRoot
{
  [DynamoDBHashKey("Id")]
  public Guid Id { get; protected set; } = Guid.NewGuid();

  [DynamoDBRangeKey("StoreId")]
  [DynamoDBGlobalSecondaryIndexRangeKey("storeId-index")]
  public Guid StoreId { get; private set; }

  private readonly List<OrderItem> _items = new();
  // [DynamoDBProperty("Items")]
  public IEnumerable<OrderItem> Items => _items.AsReadOnly();

  // [DynamoDBProperty("Status")]
  public OrderStatus Status { get; private set; } = OrderStatus.Initiated;

  [DynamoDBProperty("Total")]
  public decimal Total => Items.Sum(x => x.Price);

  public Order()
  {
    // Parameterless constructor for DynamoDB
  }

  public Order(Guid storeId)
  {
    StoreId = Guard.Against.Expression(x => x == Guid.Empty, storeId, "storeId");
  }

  public void AddItem(OrderItem item)
  {
    Guard.Against.Null(item, "item");
    _items.Add(item);

    var domainEvent = new NewOrderItemAddedEvent(this, item);
    base.RegisterDomainEvent(domainEvent);
  }

  public void UpdateStatus()
  {
    if (Status == OrderStatus.Initiated && Items.Count() > 0)
    {
      Status = OrderStatus.Submitted;
      var domainEvent = new OrderSubmittedEvent(this);
      RegisterDomainEvent(domainEvent);
    }
    else if (Status == OrderStatus.Submitted)
    {
      Status = OrderStatus.InProgress;
    }
    else
    {
      Status = OrderStatus.Complete;
      var domainEvent = new OrderCompleteEvent(this);
      RegisterDomainEvent(domainEvent);
    }
  }

  public override string ToString()
  {
    var itemNames = this.Items.Select(x => x.ProductName).ToList();
    var itemsLabel = string.Join(", ", itemNames);
    return $"{Id}: Status: {Status?.Name}; Items: {itemsLabel}; Total: {Total.ToString("C")}";
  }
}
