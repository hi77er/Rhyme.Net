using System.Text.Json;
using Rhyme.Net.Core.Domain.OrderAggregate;
using Rhyme.Net.Core.Sourcing;
using Rhyme.Net.Core.Sourcing.Payloads;
using Xunit;

namespace Rhyme.Net.Tests.Core.Domain.OrderAggregate;

public class AppendEventTests
{
    [Fact]
    public void AppendEvent_ShouldApplyOrderInitiatedEvent()
    {
        // Arrange
        var order = new Order();
        var payload = new OrderInitiatedPayload
        {
            OrderId = Guid.NewGuid(),
            StoreId = Guid.NewGuid()
        };

        var evt = new Event
        {
            AggregateName = "Order",
            AggregateId = payload.OrderId.ToString()!,
            Name = EventName.OrderInitiated,
            Payload = JsonSerializer.Serialize(payload),
            SequenceNumber = 1,
            Issuer = EventIssuer.Customer,
            IssuedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        order.AppendEvent(evt);

        // Assert
        Assert.Equal(payload.OrderId, order.Id);
        Assert.Equal(payload.StoreId, order.StoreId);
    }

    [Fact]
    public void AppendEvent_ShouldApplyOrderSubmittedEvent()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var evt = new Event
        {
            AggregateName = "Order",
            AggregateId = order.Id.ToString(),
            Name = EventName.OrderSubmitted,
            Payload = "{}",
            SequenceNumber = 2,
            Issuer = EventIssuer.Customer,
            IssuedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        order.AppendEvent(evt);

        // Assert
        Assert.Equal(OrderStatus.Submitted, order.Status);
    }

    [Fact]
    public void AppendEvent_ShouldApplyOrderCompletedEvent()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.Status = OrderStatus.InProgress;
        var evt = new Event
        {
            AggregateName = "Order",
            AggregateId = order.Id.ToString(),
            Name = EventName.OrderCompleted,
            Payload = "{}",
            SequenceNumber = 3,
            Issuer = EventIssuer.Barista,
            IssuedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        order.AppendEvent(evt);

        // Assert
        Assert.Equal(OrderStatus.Complete, order.Status);
    }

    [Fact]
    public void AppendEvent_ShouldApplyOrderItemAddedEvent()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var payload = new OrderItemAddedPayload
        {
            Item = new OrderItem("TestProduct", "TestDescription", 15.99m)
        };

        var evt = new Event
        {
            AggregateName = "Order",
            AggregateId = order.Id.ToString(),
            Name = EventName.OrderItemAdded,
            Payload = JsonSerializer.Serialize(payload),
            SequenceNumber = 4,
            Issuer = EventIssuer.Barista,
            IssuedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        order.AppendEvent(evt);

        // Assert
        Assert.Single(order.Items);
        Assert.Equal("TestProduct", order.Items[0].ProductName);
    }

    [Fact]
    public void AppendEvent_ShouldThrowException_WhenEventPayloadIsInvalid()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var evt = new Event
        {
            AggregateName = "Order",
            AggregateId = order.Id.ToString(),
            Name = EventName.OrderInitiated,
            Payload = "InvalidPayload",
            SequenceNumber = 5,
            Issuer = EventIssuer.Customer,
            IssuedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        Assert.Throws<JsonException>(() => order.AppendEvent(evt));
    }
}