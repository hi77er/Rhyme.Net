using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Menus.Update;

public record UpdateMenuCommand(int MenuId, string NewTitle) : ICommand<Result<MenuDTO>>;
