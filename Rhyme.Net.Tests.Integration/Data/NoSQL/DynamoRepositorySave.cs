using Rhyme.Net.Core.Domain.OrderAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Integration.Data.NoSQL;

public class DynamoRepositorySave : DynamoDbTestFixture<Order>
{

  [Fact]
  public async Task SaveAsync_ShouldSaveItem()
  {
    // Arrange
    var repository = GetRepository();
    var testEntity = SampleData.GetTestOrders().First();

    // Act
    await repository.SaveAsync(testEntity);
    var retrievedEntity = await repository.GetByIdAsync(testEntity.Id, testEntity.StoreId);

    // Assert
    Assert.NotNull(retrievedEntity);
    Assert.Equal(testEntity.Id, retrievedEntity.Id);
    Assert.Equal(testEntity.StoreId, retrievedEntity.StoreId);
    Assert.Equal(testEntity.Status, retrievedEntity.Status);

    // Clean up
    await repository.DeleteAsync(retrievedEntity);
  }

}