using Amazon.DynamoDBv2.DataModel;

public class DynamoRepository<T, TId> where T : class
{
  private DynamoDBOperationConfig _config => new DynamoDBOperationConfig()
  {
    OverrideTableName = ConstructTableName()
  };

  private readonly IDynamoDBContext _dbContext;

  public DynamoRepository(IDynamoDBContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<IEnumerable<T>> GetAllAsync()
    => await _dbContext
      .ScanAsync<T>(new List<ScanCondition>(), _config)
      .GetRemainingAsync();
      
  public async Task<IEnumerable<T>> QueryAsync(IEnumerable<ScanCondition> conditions)
    => await _dbContext.ScanAsync<T>(conditions, _config).GetRemainingAsync();
  public async Task<T> GetByIdAsync(Guid id) => await _dbContext.LoadAsync<T>(id, _config);
  public async Task<T> GetByIdAsync(Guid id, Guid hashKey) => await _dbContext.LoadAsync<T>(id, hashKey, _config);
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