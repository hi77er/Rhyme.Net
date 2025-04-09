using Ardalis.Result;

namespace Rhyme.Net.UseCases.Menus.AddBrochure;

/// <summary>
/// Create a new Menu.
/// </summary>
/// <param name="StoreId"></param>
/// <param name="MenuId"></param>
/// <param name="Title"></param>
public record AddBrochureCommand(Guid StoreId, Guid MenuId, string Title) : Ardalis.SharedKernel.ICommand<Result<Guid>>;