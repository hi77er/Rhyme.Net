using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Stores.ListShallow;

public class ListStoresShallowHandler(IListStoresShallowQueryService service)
  : IQueryHandler<ListStoresShallowQuery, Result<IEnumerable<StoreDTO>>>
{
  private readonly IListStoresShallowQueryService _service = service;

  public async Task<Result<IEnumerable<StoreDTO>>> Handle(ListStoresShallowQuery query, CancellationToken cancellationToken)
  {
    var result = await _service.ListAsync();

    return Result.Success(result);
  }
}