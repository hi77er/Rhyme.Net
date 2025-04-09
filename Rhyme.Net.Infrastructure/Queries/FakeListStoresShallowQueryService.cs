using Rhyme.Net.UseCases.Stores;
using Rhyme.Net.UseCases.Stores.ListShallow;

namespace Rhyme.Net.Infrastructure.Data.EF.Queries;

public class FakeListStoresShallowQueryService : IListStoresShallowQueryService
{
  public async Task<IEnumerable<StoreDTO>> ListAsync()
  {
    var testStore1 = new StoreDTO(Guid.NewGuid(), "Test Store 1");
    var testStore2 = new StoreDTO(Guid.NewGuid(), "Test Store 2");
    var testStore3 = new StoreDTO(Guid.NewGuid(), "Test Store 3");

    return await Task.FromResult(new List<StoreDTO> { testStore1, testStore2, testStore3 });
  }
}
