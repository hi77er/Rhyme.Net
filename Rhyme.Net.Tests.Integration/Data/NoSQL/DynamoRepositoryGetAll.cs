using Rhyme.Net.Core.Domain.OrderAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Integration.Data.NoSQL;

public class DynamoRepositoryGet : DynamoDbTestFixture<Order>
{

  [Fact]
  public async Task GetByIdAsync_ShouldRetrieveItem()
  {
    // Arrange
    var testEntity = SampleData.GetTestOrders().First();
    var repository = GetOrdersRepository();
    await repository.SaveAsync(testEntity);

    // Act
    var retrievedEntity = await repository.GetByIdAsync(testEntity.Id, testEntity.StoreId);

    // Assert
    Assert.NotNull(retrievedEntity);
    Assert.Equal(testEntity.Id, retrievedEntity.Id);

    // Clean up
    await repository.DeleteAsync(retrievedEntity);
  }

  [Fact]
  public async Task GetAllAsync_ShouldRetrieveAllItems()
  {
    // Arrange
    var entity1 = SampleData.GetTestOrders().First();
    var entity2 = SampleData.GetTestOrders().Skip(1).First();
    var repository = GetOrdersRepository();
    await repository.SaveAsync(entity1);
    await repository.SaveAsync(entity2);

    // Act
    var allEntities = await repository.GetAllAsync();

    // Assert
    Assert.Contains(allEntities, e => e.Id == entity1.Id);
    Assert.Contains(allEntities, e => e.Id == entity2.Id);

    // Clean up
    await repository.DeleteAsync(entity1);
    await repository.DeleteAsync(entity2);
  }

}