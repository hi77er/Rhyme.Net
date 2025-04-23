using System.Text.Json;
using Rhyme.Net.Core.Domain.OrderAggregate;
using Rhyme.Net.Core.Sourcing;
using Rhyme.Net.Core.Sourcing.Payloads;
using Xunit;

namespace Rhyme.Net.Tests.Unit.Core.Domain.OrderAggregate;

public class EventSourcingTests
{
    [Fact]
    public void AppendEvent_ShouldProcessMultipleEvents_AndReflectFinalState()
    {
        // Arrange
        var order = new Order();
        var orderId = Guid.NewGuid();
        var storeId = Guid.NewGuid();
        var item1 = new OrderItem("Laptop", "High-end laptop", 1500m);
        var item2 = new OrderItem("Mouse", "Wireless mouse", 50m);

        var events = new List<Event>
        {
            // Order initiated
            new Event
            {
                AggregateName = "Order",
                AggregateId = orderId.ToString(),
                Name = EventName.OrderInitiated,
                Payload = JsonSerializer.Serialize(new OrderInitiatedPayload { OrderId = orderId, StoreId = storeId }),
                SequenceNumber = 1,
                Issuer = EventIssuer.Customer,
                IssuedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            // Item 1 added
            new Event
            {
                AggregateName = "Order",
                AggregateId = orderId.ToString(),
                Name = EventName.OrderItemAdded,
                Payload = JsonSerializer.Serialize(new OrderItemAddedPayload { Item = item1 }),
                SequenceNumber = 2,
                Issuer = EventIssuer.Customer,
                IssuedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            // Item 2 added
            new Event
            {
                AggregateName = "Order",
                AggregateId = orderId.ToString(),
                Name = EventName.OrderItemAdded,
                Payload = JsonSerializer.Serialize(new OrderItemAddedPayload { Item = item2 }),
                SequenceNumber = 3,
                Issuer = EventIssuer.Customer,
                IssuedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            // Order submitted
            new Event
            {
                AggregateName = "Order",
                AggregateId = orderId.ToString(),
                Name = EventName.OrderSubmitted,
                Payload = "{}",
                SequenceNumber = 4,
                Issuer = EventIssuer.Customer,
                IssuedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            // Order completed
            new Event
            {
                AggregateName = "Order",
                AggregateId = orderId.ToString(),
                Name = EventName.OrderCompleted,
                Payload = "{}",
                SequenceNumber = 5,
                Issuer = EventIssuer.Barista,
                IssuedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            }
        };

        // Act
        foreach (var evt in events)
        {
            order.AppendEvent(evt);
        }

        // Assert
        Assert.Equal(orderId, order.Id);
        Assert.Equal(storeId, order.StoreId);
        Assert.Equal(OrderStatus.Complete, order.Status); // Final state should be 'Complete'
        Assert.Equal(2, order.Items.Count);
        Assert.Contains(order.Items, i => i.ProductName == item1.ProductName && i.Price == item1.Price);
        Assert.Contains(order.Items, i => i.ProductName == item2.ProductName && i.Price == item2.Price);
    }
}