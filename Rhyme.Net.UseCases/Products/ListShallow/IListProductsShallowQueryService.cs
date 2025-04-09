namespace Rhyme.Net.UseCases.Products.ListShallow;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListProductsShallowQueryService
{
  Task<IEnumerable<ProductDTO>> ListAsync();
}
