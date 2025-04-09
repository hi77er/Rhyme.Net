using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Stores.Delete;

public record DeleteStoreCommand(Guid StoreId) : ICommand<Result>;
