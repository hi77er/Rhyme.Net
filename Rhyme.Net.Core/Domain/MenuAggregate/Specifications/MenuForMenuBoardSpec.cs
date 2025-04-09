using Ardalis.Specification;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Specifications;

public class MenuForMenuBoardSpec : Specification<Menu>
{
  public MenuForMenuBoardSpec(Guid storeId)
  {
    Query
        .Where(menu => menu.StoreId == storeId)
        .Include(menu => menu.Brochures);
  }
}
