using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.OrderAggregate;

namespace Rhyme.Net.UseCases.Orders.GetByStore;

public class GetByStoreHandler(IReadRepository<Order> repository)
  : IQueryHandler<GetByStoreQuery, Result<IEnumerable<OrderDTO>>>
{
  private readonly IReadRepository<Order> _repository = repository;

  public async Task<Result<IEnumerable<OrderDTO>>> Handle(GetByStoreQuery query, CancellationToken cancellationToken)
  {
    var entity = await _repository.ListAsync(cancellationToken);
    if (entity == null) return Result.NotFound();

    var result = entity
      .Select(x => new OrderDTO(x.Id, x.StoreId, x.Total))
      .AsEnumerable();

    return Result.Success(result);
  }
}