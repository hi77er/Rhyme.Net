using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Menus.GetForPos;

public record GetMenuForPosQuery(Guid StoreId) : IQuery<Result<MenuForPosDTO>>;
