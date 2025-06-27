using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Rhyme.Net.Core.Domain.OrderAggregate;
using Rhyme.Net.Core.Interfaces;

public class DynamoRepository<T, TId> where T : class
{
  protected readonly Dictionary<string, IList<IEvent>> _eventStreams = new();
  protected readonly string _tableName;

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

  private string ConstructTableName()
  {
    var env = Environment.GetEnvironmentVariable("ENV") ?? "dev";
    var entityNameLowerCase = typeof(T).Name.ToLower();
    var pluralized = entityNameLowerCase.EndsWith("s") ? entityNameLowerCase : $"{entityNameLowerCase}s";
    var newTableName = $"{pluralized}-{env}";
    return newTableName;
  }

}