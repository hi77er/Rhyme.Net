using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Products.ListShallow;

public class ListProductsShallowHandler(IListProductsShallowQueryService query)
  : IQueryHandler<ListProductsShallowQuery, Result<IEnumerable<ProductDTO>>>
{
  private readonly IListProductsShallowQueryService _query = query;

  public async Task<Result<IEnumerable<ProductDTO>>> Handle(ListProductsShallowQuery request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}