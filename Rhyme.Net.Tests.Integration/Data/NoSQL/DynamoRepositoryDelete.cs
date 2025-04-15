using Rhyme.Net.Core.Domain.OrderAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Integration.Data.NoSQL;

public class DynamoRepositoryDelete : DynamoDbTestFixture<Order>
{

  [Fact]
  public async Task DeleteAsync_ShouldDeleteItem()
  {
    // Arrange
    var testEntity = SampleData.GetTestOrders().First();
    var repository = GetRepository();
    await repository.SaveAsync(testEntity);

    // Act
    await repository.DeleteAsync(testEntity);
    var deletedEntity = await repository.GetByIdAsync(testEntity.Id, testEntity.StoreId);

    // Assert
    Assert.Null(deletedEntity);
  }

}