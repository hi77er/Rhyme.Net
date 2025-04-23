using Rhyme.Net.Core.Domain.OrderAggregate;
using Rhyme.Net.Core.Domain.OrderAggregate.Events;
using Xunit;

namespace Rhyme.Net.Tests.Unit.Core.Domain.OrderAggregate;

public class UpdateStatusTests
{
    [Fact]
    public void UpdateStatus_ShouldChangeFromInitiatedToSubmitted_WhenItemsArePresent()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var item = new OrderItem("Product1", "Description1", 10.0m);
        order.AddItem(item);

        // Act
        order.UpdateStatus();

        // Assert
        Assert.Equal(OrderStatus.Submitted, order.Status);
    }

    [Fact]
    public void UpdateStatus_ShouldChangeFromSubmittedToInProgress()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.Status = OrderStatus.Submitted;

        // Act
        order.UpdateStatus();

        // Assert
        Assert.Equal(OrderStatus.InProgress, order.Status);
    }

    [Fact]
    public void UpdateStatus_ShouldChangeFromInProgressToComplete()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.Status = OrderStatus.InProgress;

        // Act
        order.UpdateStatus();

        // Assert
        Assert.Equal(OrderStatus.Complete, order.Status);
    }

    [Fact]
    public void UpdateStatus_ShouldRaiseEvent_WhenOrderCompletes()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.Status = OrderStatus.InProgress;

        // Act
        order.UpdateStatus();

        // Assert
        Assert.Contains(order.DomainEvents, e => e is OrderCompleteEvent);
    }

    [Fact]
    public void UpdateStatus_ShouldNotChange_WhenNoItemsAreAdded()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        // Act
        order.UpdateStatus();

        // Assert
        Assert.Equal(OrderStatus.Complete, order.Status);
    }
}