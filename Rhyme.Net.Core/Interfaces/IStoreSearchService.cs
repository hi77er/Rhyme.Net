using Ardalis.Result;
using Rhyme.Net.Core.Domain.StoreAggregate;

namespace Rhyme.Net.Core.Interfaces;

public interface IStoreSearchService
{
  Task<Result<Store>> GetStoreAsync(int storeId);
  Task<Result<List<Store>>> GetAllStoresAsync(string searchString);
}
