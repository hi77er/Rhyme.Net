using Ardalis.Result;

namespace Rhyme.Net.UseCases.Menus.Create;

/// <summary>
/// Create a new Menu.
/// </summary>
/// <param name="StoreId"></param>
/// <param name="Title"></param>
public record CreateMenuCommand(Guid StoreId, string Title) : Ardalis.SharedKernel.ICommand<Result<Guid>>;