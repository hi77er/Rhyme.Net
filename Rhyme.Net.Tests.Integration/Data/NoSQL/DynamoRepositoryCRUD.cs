using Rhyme.Net.Core.Domain.OrderAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Integration.Data.NoSQL;

public class DynamoRepositoryCRUD : DynamoDbTestFixture<Order>
{

  [Fact]
  public async Task SaveGetDelete_ShouldSaveGetDeleteItem()
  {
    // Arrange
    var repository = GetOrdersRepository();
    var testEntity = SampleData.GetTestOrders().First();

    // Act
    await repository.SaveAsync(testEntity);
    var retrievedEntity = await repository.GetByIdAsync(testEntity.Id, testEntity.StoreId);

    // Assert
    Assert.NotNull(retrievedEntity);
    Assert.Equal(testEntity.Id, retrievedEntity.Id);
    Assert.Equal(testEntity.StoreId, retrievedEntity.StoreId);
    Assert.Equal(testEntity.Status, retrievedEntity.Status);

    testEntity.UpdateStatus();
    await repository.SaveAsync(testEntity);

    var updatedEntity = await repository.GetByIdAsync(testEntity.Id, testEntity.StoreId);
    Assert.NotNull(updatedEntity);
    Assert.Equal(testEntity.Id, updatedEntity.Id);
    Assert.Equal(testEntity.StoreId, updatedEntity.StoreId);
    Assert.Equal(testEntity.Status, updatedEntity.Status);

    // Clean up
    await repository.DeleteAsync(retrievedEntity);
  }

}