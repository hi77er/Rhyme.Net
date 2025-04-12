using Amazon.DynamoDBv2.DataModel;

public class DynamoRepository<T> where T : class
{
  private readonly IDynamoDBContext _dbContext;

  public DynamoRepository(IDynamoDBContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<T> GetByIdAsync(object id)
  {
    return await _dbContext.LoadAsync<T>(id);
  }

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _dbContext.ScanAsync<T>(new List<ScanCondition>()).GetRemainingAsync();
  }

  public async Task SaveAsync(T item)
  {
    await _dbContext.SaveAsync(item);
  }

  public async Task DeleteAsync(T item)
  {
    await _dbContext.DeleteAsync(item);
  }

  public async Task<IEnumerable<T>> QueryAsync(IEnumerable<ScanCondition> conditions)
  {
    return await _dbContext.ScanAsync<T>(conditions).GetRemainingAsync();
  }

  //Add other methods as needed, such as Query, Scan etc.
}