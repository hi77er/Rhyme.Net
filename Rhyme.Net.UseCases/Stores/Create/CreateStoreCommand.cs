using Ardalis.Result;

namespace Rhyme.Net.UseCases.Stores.Create;

/// <summary>
/// Create a new Store.
/// </summary>
/// <param name="Name"></param>
public record CreateStoreCommand(string Name) : Ardalis.SharedKernel.ICommand<Result<Guid>>;