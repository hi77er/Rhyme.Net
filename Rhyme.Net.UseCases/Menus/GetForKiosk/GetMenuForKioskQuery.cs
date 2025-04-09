using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Menus.GetForKiosk;

public record GetMenuForKioskQuery(Guid StoreId) : IQuery<Result<MenuForKioskDTO>>;
