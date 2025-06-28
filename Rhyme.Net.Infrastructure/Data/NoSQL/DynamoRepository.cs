using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
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

  
  public async Task WriteBatchAsync(IEnumerable<T> entities)
  {
    var batch = new List<WriteRequest>();

    foreach (var entity in entities)
    {
      var item = entity.ToAttributeValues();

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