using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Stores.Update;

public record UpdateStoreCommand(int StoreId, string NewName) : ICommand<Result<StoreDTO>>;
