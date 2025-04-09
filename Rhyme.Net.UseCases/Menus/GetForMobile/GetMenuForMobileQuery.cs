using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Menus.GetForMobile;

public record GetMenuForMobileQuery(Guid StoreId) : IQuery<Result<MenuForMobileDTO>>;
