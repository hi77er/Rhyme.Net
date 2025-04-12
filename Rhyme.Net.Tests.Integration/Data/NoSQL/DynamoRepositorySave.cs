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
    var testEntity = new Order(Guid.NewGuid());

    // Act
    await repository.SaveAsync(testEntity);
    var retrievedEntity = await repository.GetByIdAsync(testEntity.Id);

    // Assert
    Assert.NotNull(retrievedEntity);
    Assert.Equal(testEntity.Id, retrievedEntity.Id);

    // Clean up
    await repository.DeleteAsync(retrievedEntity);
  }

}