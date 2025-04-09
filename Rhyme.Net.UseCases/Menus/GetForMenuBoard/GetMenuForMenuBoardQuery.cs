using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Menus.GetForMenuBoard;

public record GetMenuForMenuBoardQuery(Guid StoreId) : IQuery<Result<MenuForMenuBoardDTO>>;
