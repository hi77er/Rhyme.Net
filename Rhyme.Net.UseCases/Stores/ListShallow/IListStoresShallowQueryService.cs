namespace Rhyme.Net.UseCases.Stores.ListShallow;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListStoresShallowQueryService
{
  Task<IEnumerable<StoreDTO>> ListAsync();
}
