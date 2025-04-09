using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Stores.ListShallow;

public class ListStoresShallowHandler(IListStoresShallowQueryService query)
  : IQueryHandler<ListStoresShallowQuery, Result<IEnumerable<StoreDTO>>>
{
  private readonly IListStoresShallowQueryService _query = query;

  public async Task<Result<IEnumerable<StoreDTO>>> Handle(ListStoresShallowQuery request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}