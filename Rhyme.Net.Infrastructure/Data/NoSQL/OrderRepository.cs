using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Rhyme.Net.Core.Domain.OrderAggregate;

namespace Rhyme.Net.Infrastructure.Data.NoSQL;

public class OrderRepository : DynamoRepository<Order, Guid>
{
  public OrderRepository(IAmazonDynamoDB dbClient, IDynamoDBContext dbContext) : base(dbClient, dbContext)
  {
  }

  public Order Get(string aggregateName, Guid aggregateId)
  {
    var streamId = $"{aggregateName}-{aggregateId}";
    var orderAggregate = new Order()
    {
      Id = aggregateId,
    };

    if (_eventStreams.ContainsKey(streamId))
    {
      var events = _eventStreams[streamId];
      foreach (var evt in events)
      {
        orderAggregate.AppendEvent(evt);
      }
    }

    return orderAggregate;
  }
}