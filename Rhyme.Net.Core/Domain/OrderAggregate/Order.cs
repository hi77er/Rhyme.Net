using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.DataModel;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Converters;
using Rhyme.Net.Core.Domain.OrderAggregate.Events;
using Rhyme.Net.Core.Interfaces;
using Rhyme.Net.Core.Sourcing;
using Rhyme.Net.Core.Sourcing.Payloads;

namespace Rhyme.Net.Core.Domain.OrderAggregate;

[DynamoDBTable("orders")]
public class Order : DynamoDbEntity, IAggregateRoot
{
  [DynamoDBHashKey("id")]
  [DynamoDBProperty(Converter = typeof(GuidConverter))]
  public Guid Id { get; set; } = Guid.NewGuid();

  [DynamoDBRangeKey("storeId")]
  [DynamoDBGlobalSecondaryIndexRangeKey("storeId-index")]
  [DynamoDBProperty(Converter = typeof(GuidConverter))]
  public Guid StoreId { get; set; }

  [DynamoDBProperty("items", Converter = typeof(ListConverter<OrderItem>))]
  public IList<OrderItem> Items { get; set; } = new List<OrderItem>();

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

  public void AppendEvent(IEvent evt)
  {
    ValidateEvent(evt);

    switch (evt.Name)
    {
      case EventName.OrderInitiated:
        ApplyOrderInitiatedEvent(evt);
        break;
      case EventName.OrderItemAdded:
        ApplyOrderItemAddedEvent(evt);
        break;
      case EventName.OrderSubmitted:
        ApplyOrderSubmittedEvent(evt);
        break;
      case EventName.OrderCompleted:
        ApplyOrderCompletedEvent(evt);
        break;
      case EventName.OrderItemRemoved:
        ApplyOrderItemRemovedEvent(evt);
        break;
      case EventName.OrderItemModifierAdjusted:
        ApplyOrderItemModifierAdjustedEvent(evt);
        break;
      case EventName.OrderItemModifierRemoved:
        ApplyOrderItemModifierRemovedEvent(evt);
        break;
    }
  }

  private void ApplyOrderInitiatedEvent(IEvent evt)
  {
    var payload = JsonSerializer.Deserialize<OrderInitiatedPayload>(evt.Payload);
    ValidatePayload(payload);

    Id = payload!.OrderId!.Value;
    StoreId = payload!.StoreId!.Value;
  }

  private void ApplyOrderSubmittedEvent(IEvent evt)
  {
    Guard.Against.Expression(x => !x.Equals(EventName.OrderSubmitted), evt.Name!.Value, nameof(evt.Name.Value));
    Status = OrderStatus.Submitted;
  }

  private void ApplyOrderCompletedEvent(IEvent evt)
  {
    Guard.Against.Expression(x => !x.Equals(EventName.OrderCompleted), evt.Name!.Value, nameof(evt.Name.Value));
    Status = OrderStatus.Complete;
  }

  private void ApplyOrderItemAddedEvent(IEvent evt)
  {
    var payload = JsonSerializer.Deserialize<OrderItemAddedPayload>(evt.Payload);
    ValidatePayload(payload);

    var item = new OrderItem(
      payload!.Item!.ProductName,
      payload.Item.ProductDescription,
      payload.Item.Price);
    Items.Add(item);
  }

  private void ApplyOrderItemModifierAdjustedEvent(IEvent evt)
  {
    var payload = JsonSerializer.Deserialize<ModifierAdjustedPayload>(evt.Payload);
    ValidatePayload(payload);

    var item = Items.FirstOrDefault(x => x.Id == payload!.ItemId!.Value);
    if (item != null)
    {
      item.AdjustModifier(payload!.SelectedModifier!);
    }
  }

  private void ApplyOrderItemModifierRemovedEvent(IEvent evt)
  {
    var payload = JsonSerializer.Deserialize<ModifierRemovedPayload>(evt.Payload);
    ValidatePayload(payload);

    var item = Items.FirstOrDefault(x => x.Id == payload!.ItemId!.Value);
    if (item != null)
    {
      item.RemoveModifier(payload!.SelectedModifier!);
    }
  }

  private void ApplyOrderItemAddOnAdjustedEvent(IEvent evt)
  {
    var payload = JsonSerializer.Deserialize<AddOnAdjustedPayload>(evt.Payload);
    ValidatePayload(payload);

    var item = Items.FirstOrDefault(x => x.Id == payload!.ItemId!.Value);
    if (item != null)
    {
      item.AdjustAddOn(payload!.SelectedAddOn!);
    }
  }

  private void ApplyOrderItemAddOnRemovedEvent(IEvent evt)
  {
    var payload = JsonSerializer.Deserialize<AddOnRemovedPayload>(evt.Payload);
    ValidatePayload(payload);

    var item = Items.FirstOrDefault(x => x.Id == payload!.ItemId!.Value);
    if (item != null)
    {
      item.RemoveAddOn(payload!.SelectedAddOn!);
    }
  }

  private void ApplyOrderItemRemovedEvent(IEvent evt)
  {
    var payload = JsonSerializer.Deserialize<OrderItemRemovedPayload>(evt.Payload);
    ValidatePayload(payload);

    var item = Items.FirstOrDefault(x => x.Id == payload!.ItemId!.Value);
    if (item != null)
      Items.Remove(item);
  }

  private void ValidateEvent(IEvent evt)
  {
    Guard.Against.Null(evt, nameof(evt));
    Guard.Against.Null(evt.Name, nameof(evt.Name));
    Guard.Against.NullOrEmpty(evt.Payload, nameof(evt.Payload));
  }

  private void ValidatePayload(OrderInitiatedPayload? payload)
  {
    Guard.Against.Null(payload, nameof(payload));
    Guard.Against.Null(payload.OrderId, nameof(payload.OrderId));
    Guard.Against.Null(payload.StoreId, nameof(payload.StoreId));
  }

  private void ValidatePayload(OrderItemAddedPayload? payload)
  {
    Guard.Against.Null(payload, nameof(payload));
    Guard.Against.Null(payload.Item, nameof(payload.Item));
    Guard.Against.Negative(payload.Item.Price, nameof(payload.Item.Price));
  }

  private void ValidatePayload(OrderItemRemovedPayload? payload)
  {
    Guard.Against.Null(payload, nameof(payload));
    Guard.Against.Null(payload.ItemId, nameof(payload.ItemId));
    Guard.Against.Default(payload.ItemId, nameof(payload.ItemId));
  }
  
  private void ValidatePayload(ModifierAdjustedPayload? payload)
  {
    Guard.Against.Null(payload, nameof(payload));
    Guard.Against.Null(payload.ItemId, nameof(payload.ItemId));
    Guard.Against.Default(payload.ItemId, nameof(payload.ItemId));
    Guard.Against.Default(payload.SelectedModifier, nameof(payload.SelectedModifier));
    Guard.Against.Default(payload.SelectedModifier.ModifierId, nameof(payload.SelectedModifier.ModifierId));
    Guard.Against.Default(payload.SelectedModifier.SelectedMenuItemVariantId, nameof(payload.SelectedModifier.SelectedMenuItemVariantId));
  }

  private void ValidatePayload(ModifierRemovedPayload? payload)
  {
    Guard.Against.Null(payload, nameof(payload));
    Guard.Against.Null(payload.ItemId, nameof(payload.ItemId));
    Guard.Against.Default(payload.ItemId, nameof(payload.ItemId));
    Guard.Against.Default(payload.SelectedModifier, nameof(payload.SelectedModifier));
    Guard.Against.Default(payload.SelectedModifier.ModifierId, nameof(payload.SelectedModifier.ModifierId));
    Guard.Against.Default(payload.SelectedModifier.SelectedMenuItemVariantId, nameof(payload.SelectedModifier.SelectedMenuItemVariantId));
  }

  private void ValidatePayload(AddOnAdjustedPayload? payload)
  {
    Guard.Against.Null(payload, nameof(payload));
    Guard.Against.Null(payload.ItemId, nameof(payload.ItemId));
    Guard.Against.Default(payload.ItemId, nameof(payload.ItemId));
    Guard.Against.Default(payload.SelectedAddOn, nameof(payload.SelectedAddOn));
    Guard.Against.Default(payload.SelectedAddOn.AddOnId, nameof(payload.SelectedAddOn.AddOnId));
  }

  private void ValidatePayload(AddOnRemovedPayload? payload)
  {
    Guard.Against.Null(payload, nameof(payload));
    Guard.Against.Null(payload.ItemId, nameof(payload.ItemId));
    Guard.Against.Default(payload.ItemId, nameof(payload.ItemId));
    Guard.Against.Default(payload.SelectedAddOn, nameof(payload.SelectedAddOn));
    Guard.Against.Default(payload.SelectedAddOn.AddOnId, nameof(payload.SelectedAddOn.AddOnId));
  }


}
