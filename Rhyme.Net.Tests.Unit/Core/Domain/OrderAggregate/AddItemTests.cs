using Rhyme.Net.Core.Domain.OrderAggregate;
using Rhyme.Net.Core.Domain.OrderAggregate.Events;
using Xunit;

namespace Rhyme.Net.Tests.Core.Domain.OrderAggregate;

public class AddItemTests
{
    [Fact]
    public void AddItem_ShouldAddItemToOrder()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var item = new OrderItem("Product1", "Description1", 10.0m);

        // Act
        order.AddItem(item);

        // Assert
        Assert.Contains(item, order.Items);
        Assert.Single(order.Items);
    }

    [Fact]
    public void AddItem_ShouldRaiseDomainEvent()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var item = new OrderItem("Product1", "Description1", 10.0m);

        // Act
        order.AddItem(item);

        // Assert
        Assert.Contains(order.DomainEvents, e => e is NewOrderItemAddedEvent);
    }

    [Fact]
    public void AddItem_ShouldContainExactOrderItemInCollection()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        var item = new OrderItem("ProductX", "High-quality description", 25.99m);

        // Act
        order.AddItem(item);

        // Assert
        var retrievedItem = order.Items.FirstOrDefault(i => i.ProductName == "ProductX" && i.Price == 25.99m);
        Assert.NotNull(retrievedItem);
        Assert.Equal(item, retrievedItem);
    }
}