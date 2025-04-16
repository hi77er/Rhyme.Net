using Rhyme.Net.Core.Domain.OrderAggregate;
using Xunit;

namespace Rhyme.Net.Tests.Integration.Data.NoSQL;

public class EventCRUD : DynamoDbTestFixture<Order>
{

  [Fact]
  public async Task SaveGetDelete_ShouldSaveGetDeleteEvent()
  {
    // Arrange
    var repository = GetEventsRepository();
    var testEntity = SampleData.GetTestEvents().First();

    // Act
    await repository.SaveAsync(testEntity);
    var retrievedEntity = await repository.GetByIdAsync(testEntity!.Name!.Value.ToString(), testEntity.AggregateId);

    // Assert
    Assert.NotNull(retrievedEntity);
    Assert.Equal(testEntity.AggregateId, retrievedEntity.AggregateId);
    Assert.Equal(testEntity.AggregateName, retrievedEntity.AggregateName);
    Assert.Equal(testEntity.Name, retrievedEntity.Name);
    Assert.Equal(testEntity.Payload, retrievedEntity.Payload);
    Assert.Equal(testEntity.SequenceNumber, retrievedEntity.SequenceNumber);
    Assert.Equal(testEntity.Issuer, retrievedEntity.Issuer);
    Assert.Equal(testEntity.IssuedAt, retrievedEntity.IssuedAt);
    Assert.Equal(testEntity.CreatedAt, retrievedEntity.CreatedAt);

    // Clean up
    await repository.DeleteAsync(retrievedEntity);
  }

}