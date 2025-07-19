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

  public async Task<ScanResponse> ScanAsync(Dictionary<string, AttributeValue> lastEvaluatedKey, int limit = 1000, CancellationToken cancellationToken = default)
  {
    var scanRequest = new ScanRequest
    {
      TableName = _tableName,
      ExclusiveStartKey = lastEvaluatedKey,
      Limit = limit
    };

    var scanResponse = await _dbClient.ScanAsync(scanRequest, cancellationToken);

    return scanResponse;
  }

  public async Task FlushTableAsync(CancellationToken cancellationToken = default)
  {
    Console.WriteLine($"Deleting all items from table '{_tableName}'...");
    var lastEvaluatedKey = new Dictionary<string, AttributeValue>();

    do
    {
      var scanResponse = await ScanAsync(lastEvaluatedKey, 500, cancellationToken);
      lastEvaluatedKey = scanResponse.LastEvaluatedKey;

      await DeleteBatchAsync(scanResponse.Items, cancellationToken);
    } while (lastEvaluatedKey.Count > 0);

    Console.WriteLine("All records deleted.");
  }

  public async Task DeleteBatchAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    var batch = new List<WriteRequest>();
    var attributeValueItems = entities
      .Select(entity => entity.ToAttributeValues())
      .ToList();

    await DeleteBatchAsync(attributeValueItems, cancellationToken);
  }

  public async Task DeleteBatchAsync(IEnumerable<Dictionary<string, AttributeValue>> items, CancellationToken cancellationToken = default)
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
        await FlushBatchAsync(batch, cancellationToken);
        batch.Clear();
      }
    }

    if (batch.Count > 0)
      await FlushBatchAsync(batch, cancellationToken);
  }

  public async Task GentleWriteBatchAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    List<WriteRequest>? batch = null;
    int count = 0;

    foreach (var entity in entities)
    {
      batch = batch ?? new List<WriteRequest>();
      var item = entity.ToAttributeValues();

      batch.Add(new WriteRequest
      {
        PutRequest = new PutRequest { Item = item }
      });

      count++;

      if (batch.Count == BatchSize)
      {
        Console.WriteLine($"Writing batch of {BatchSize:N0} items to table '{_tableName}'...");
        await FlushBatchAsync(batch, cancellationToken);

        // Memory tracking after each flush
        long memoryUsed = GC.GetTotalMemory(forceFullCollection: false);
        Console.WriteLine($"Memory used [Step 3] after flush of batch {count / BatchSize}: {memoryUsed / 1048576:N0} MBs");

        batch.Clear();
        batch = null;
        GC.Collect(); // Hint to GC, not guaranteed

        // Memory tracking after each flush
        long memoryUsed2 = GC.GetTotalMemory(forceFullCollection: false);
        Console.WriteLine($"Memory used [Step 4] after GC hint: {memoryUsed2 / 1048576:N0} MBs");
      }
    }

    if ((batch?.Count ?? 0) > 0)
    {
      Console.WriteLine($"Writing final batch of {batch!.Count:N0} items to table '{_tableName}'...");
      await FlushBatchAsync(batch, cancellationToken);
    }

    // Optional: clean up memory
    batch?.Clear();
    batch = null;
    GC.Collect(); // Hint to GC, not guaranteed

    // Memory tracking after each flush
    long memoryUsed3 = GC.GetTotalMemory(forceFullCollection: false);
    Console.WriteLine($"Memory used [Step 5] after final GC hint: {memoryUsed3 / 1048576:N0} MBs");
  }

  public async Task WriteBatchAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    var batch = new List<WriteRequest>();
    var attributeValueItems = entities
      .Select(entity => entity.ToAttributeValues())
      .ToList();

    long memoryUsed = GC.GetTotalMemory(forceFullCollection: false);
    Console.WriteLine($"Memory used [Step 3]: {memoryUsed / 1048576:N0} MBs");

    Console.WriteLine($"Writing {attributeValueItems.Count:N0} items to table '{_tableName}'...");
    await WriteBatchAsync(attributeValueItems, cancellationToken);
  }

  public async Task WriteBatchAsync(IEnumerable<Dictionary<string, AttributeValue>> items, CancellationToken cancellationToken = default)
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
        await FlushBatchAsync(batch, cancellationToken);
        batch.Clear();
      }
    }

    if (batch.Count > 0)
      await FlushBatchAsync(batch, cancellationToken);
  }

  private async Task FlushBatchAsync(List<WriteRequest> batch, CancellationToken cancellationToken = default)
  {
    var request = new BatchWriteItemRequest
    {
      RequestItems = new Dictionary<string, List<WriteRequest>>
      {
        [_tableName] = batch
      }
    };

    Console.WriteLine($"Flushing batch of {batch.Count:N0} items to table '{_tableName}'...");
    var response = await _dbClient.BatchWriteItemAsync(request, cancellationToken);

    if (response.UnprocessedItems.Count > 0)
    {
      // Simple retry (can be expanded with exponential backoff)
      await Task.Delay(50);
      await FlushBatchAsync(response.UnprocessedItems[_tableName], cancellationToken);
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