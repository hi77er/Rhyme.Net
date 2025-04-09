using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Menus.Delete;

public record DeleteMenuCommand(Guid MenuId) : ICommand<Result>;
