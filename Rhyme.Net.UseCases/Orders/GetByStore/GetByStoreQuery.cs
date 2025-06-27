using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Orders.GetByStore;

public record GetByStoreQuery(Guid StoreId, int Skip, int Take) : IQuery<Result<IEnumerable<OrderDTO>>>;
