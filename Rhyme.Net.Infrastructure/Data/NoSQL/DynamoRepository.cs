using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Rhyme.Net.Core.Interfaces;

public class DynamoRepository<T, TId> : IDynamoRepository<T, TId>
  where T : DynamoDbEntity
  where TId : notnull
{
  protected readonly Dictionary<string, IList<IEvent>> _eventStreams = new();
  protected readonly string _tableName;
  private const int BatchSize = 25;

  private DynamoDBOperationConfig _config => new DynamoDBOperationConfig()
  {
    OverrideTableName = ConstructTableName()
  };

  protected readonly IAmazonDynamoDB _dbClient;
  protected readonly IDynamoDBContext _dbContext;

  public DynamoRepository(IAmazonDynamoDB dbClient, IDynamoDBContext dbContext)
  {
    _dbClient = dbClient;
    _dbContext = dbContext;
    _tableName = _config.OverrideTableName;
  }

  public async Task<IEnumerable<T>> GetAllAsync()
    => await _dbContext
      .ScanAsync<T>(new List<ScanCondition>(), _config)
      .GetRemainingAsync();

  public async Task<IEnumerable<T>> QueryAsync(IEnumerable<ScanCondition> conditions)
    => await _dbContext.ScanAsync<T>(conditions, _config).GetRemainingAsync();
  public async Task<T> GetByIdAsync(Guid id) => await _dbContext.LoadAsync<T>(id, _config);
  public async Task<T> GetByIdAsync(Guid id, Guid hashKey) => await _dbContext.LoadAsync<T>(id, hashKey, _config);
  public async Task<T> GetByIdAsync(string id, string hashKey) => await _dbContext.LoadAsync<T>(id, hashKey, _config);
  public async Task SaveAsync(T item) => await _dbContext.SaveAsync(item, _config);
  public async Task DeleteAsync(T item) => await _dbContext.DeleteAsync(item, _config);

  public async Task FlushTableAsync()
  {
    Console.WriteLine($"Deleting all items from table '{_tableName}'...");
    var lastEvaluatedKey = new Dictionary<string, AttributeValue>();

    do
    {
      var scanRequest = new ScanRequest
      {
        TableName = _tableName,
        ExclusiveStartKey = lastEvaluatedKey,
        ProjectionExpression = "id" // Adjust this if your partition key has a different name
      };

      var scanResponse = await _dbClient.ScanAsync(scanRequest);
      lastEvaluatedKey = scanResponse.LastEvaluatedKey;

      var allItems = scanResponse.Items;
      await DeleteBatchAsync(allItems);

    } while (lastEvaluatedKey.Count > 0);

    Console.WriteLine("All records deleted.");
  }

  public async Task DeleteBatchAsync(IEnumerable<T> entities)
  {
    var batch = new List<WriteRequest>();
    var attributeValueItems = entities
      .Select(entity => entity.ToAttributeValues())
      .ToList();

    await DeleteBatchAsync(attributeValueItems);
  }

  public async Task DeleteBatchAsync(IEnumerable<Dictionary<string, AttributeValue>> items)
  {
    var batch = new List<WriteRequest>();

    foreach (var item in items)
    {
      batch.Add(new WriteRequest
      {
        DeleteRequest = new DeleteRequest { Key = item }
      });

      if (batch.Count == BatchSize)
      {
        await FlushBatchAsync(batch);
        batch.Clear();
      }
    }

    if (batch.Count > 0)
      await FlushBatchAsync(batch);
  }

  public async Task WriteBatchAsync(IEnumerable<T> entities)
  {
    var batch = new List<WriteRequest>();
    var attributeValueItems = entities
      .Select(entity => entity.ToAttributeValues())
      .ToList();

    await WriteBatchAsync(attributeValueItems);
  }

  public async Task WriteBatchAsync(IEnumerable<Dictionary<string, AttributeValue>> items)
  {
    var batch = new List<WriteRequest>();

    foreach (var item in items)
    {
      batch.Add(new WriteRequest
      {
        PutRequest = new PutRequest { Item = item }
      });

      if (batch.Count == BatchSize)
      {
        await FlushBatchAsync(batch);
        batch.Clear();
      }
    }

    if (batch.Count > 0)
      await FlushBatchAsync(batch);
  }

  private async Task FlushBatchAsync(List<WriteRequest> batch)
  {
    var request = new BatchWriteItemRequest
    {
      RequestItems = new Dictionary<string, List<WriteRequest>>
      {
        [_tableName] = batch
      }
    };

    var response = await _dbClient.BatchWriteItemAsync(request);

    if (response.UnprocessedItems.Count > 0)
    {
      // Simple retry (can be expanded with exponential backoff)
      await Task.Delay(50);
      await FlushBatchAsync(response.UnprocessedItems[_tableName]);
    }
  }

  private string ConstructTableName()
  {
    var env = Environment.GetEnvironmentVariable("ENV") ?? "dev";
    var entityNameLowerCase = typeof(T).Name.ToLower();
    var pluralized = entityNameLowerCase.EndsWith("s") ? entityNameLowerCase : $"{entityNameLowerCase}s";
    var newTableName = $"{pluralized}-{env}";
    return newTableName;
  }

}