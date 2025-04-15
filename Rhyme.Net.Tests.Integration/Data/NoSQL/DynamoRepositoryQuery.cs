using Amazon.DynamoDBv2.DataModel;
using Rhyme.Net.Core.Domain.OrderAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Integration.Data.NoSQL;

public class DynamoRepositoryQuery : DynamoDbTestFixture<Order>
{

  [Fact]
  public async Task QueryAsync_ShouldFilterItems()
  {
    // Arrange
    var matchingEntity = SampleData.GetTestOrders().First();
    var nonMatchingEntity = SampleData.GetTestOrders().Last();
    var condition = new ScanCondition("Id", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, matchingEntity.Id.ToString());
    var repository = GetOrdersRepository();

    await repository.SaveAsync(matchingEntity);
    await repository.SaveAsync(nonMatchingEntity);

    // Act
    var filteredEntities = await repository.QueryAsync(new[] { condition });

    // Assert
    Assert.Single(filteredEntities);
    Assert.Equal(matchingEntity.Id, filteredEntities.Single().Id);

    // Clean up
    await repository.DeleteAsync(matchingEntity);
    await repository.DeleteAsync(nonMatchingEntity);
  }

}