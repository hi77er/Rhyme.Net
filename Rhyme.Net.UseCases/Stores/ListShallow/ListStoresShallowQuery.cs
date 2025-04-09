using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Stores.ListShallow;

public record ListStoresShallowQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<StoreDTO>>>;
