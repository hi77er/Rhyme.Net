using Amazon.DynamoDBv2.DataModel;
using Ardalis.SharedKernel;

public interface IDynamoRepository<T, TKey> where T : class
{
  Task<IEnumerable<T>> GetAllAsync();
  Task<IEnumerable<T>> QueryAsync(IEnumerable<ScanCondition> conditions);
  Task<T> GetByIdAsync(Guid id);
  Task<T> GetByIdAsync(Guid id, Guid hashKey);
  Task<T> GetByIdAsync(string id, string hashKey);
  Task SaveAsync(T item);
  Task WriteBatchAsync(IEnumerable<T> entities);
  Task DeleteAsync(T item);
}