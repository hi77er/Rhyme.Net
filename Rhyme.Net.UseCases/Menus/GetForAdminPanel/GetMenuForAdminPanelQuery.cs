using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Rhyme.Net.UseCases.Menus.GetForAdminPanel;

public record GetMenuForAdminPanelQuery(Guid StoreId) : IQuery<Result<MenuForAdminPanelDTO>>;
