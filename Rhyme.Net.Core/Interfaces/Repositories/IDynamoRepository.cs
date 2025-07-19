using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Ardalis.SharedKernel;

public interface IDynamoRepository<T, TKey> where T : class
{
  Task<IEnumerable<T>> GetAllAsync();
  Task<IEnumerable<T>> QueryAsync(IEnumerable<ScanCondition> conditions);
  Task<T> GetByIdAsync(Guid id);
  Task<T> GetByIdAsync(Guid id, Guid hashKey);
  Task<T> GetByIdAsync(string id, string hashKey);
  Task SaveAsync(T item);
  Task<ScanResponse> ScanAsync(Dictionary<string, AttributeValue> lastEvaluatedKey, int limit = 1000, CancellationToken cancellationToken = default);
  /// <summary>
  /// This is a DynamoDB Batch write method that flushed the collection of requests and hints the GC to collect the unused memory 
  /// after each batch write has completed.
  /// </summary>
  /// <param name="entities"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task GentleWriteBatchAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
  Task WriteBatchAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
  Task DeleteBatchAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
  Task DeleteAsync(T item);
  Task FlushTableAsync(CancellationToken cancellationToken = default);

}