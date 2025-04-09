using Ardalis.Specification;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Specifications;

public class MenuForAdminPanelSpec : Specification<Menu>
{
  public MenuForAdminPanelSpec(Guid storeId)
  {
    Query
        .Where(menu => menu.StoreId == storeId)
        .Include(menu => menu.Brochures)
        .ThenInclude(brochure => brochure.MenuItems);
  }
}
