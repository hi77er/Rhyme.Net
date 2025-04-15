using Amazon.DynamoDBv2.DataModel;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.OrderAggregate.Events;

namespace Rhyme.Net.Core.Domain.OrderAggregate;

[DynamoDBTable("orders")]
public class Order : HasDomainEventsBase, IAggregateRoot
{
  [DynamoDBHashKey("id")]
  public Guid Id { get; protected set; } = Guid.NewGuid();

  [DynamoDBRangeKey("storeId")]
  [DynamoDBGlobalSecondaryIndexRangeKey("storeId-index")]
  public Guid StoreId { get; set; }

  [DynamoDBProperty("items")]
  public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

  [DynamoDBProperty("status")]
  public OrderStatus Status { get; set; } = OrderStatus.Initiated;

  public decimal Total => Items.Sum(x => x.Price);

  /// <summary>
  /// Default constructor for DynamoDB
  /// </summary>
  public Order() { }

  public Order(Guid storeId)
  {
    StoreId = Guard.Against.Expression(x => x == Guid.Empty, storeId, "storeId");
  }

  public void AddItem(OrderItem item)
  {
    Guard.Against.Null(item, "item");
    Items.Add(item);

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
    return $"{Id}: Status: {Status}; Items: {itemsLabel}; Total: {Total}";
  }
}
