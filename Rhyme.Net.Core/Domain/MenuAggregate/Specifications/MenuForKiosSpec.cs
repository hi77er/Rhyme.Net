using Ardalis.Specification;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Specifications;

public class MenuForKioskSpec : Specification<Menu>
{
  public MenuForKioskSpec(Guid storeId)
  {
    Query
        .Where(menu => menu.StoreId == storeId)
        .Include(menu => menu.Brochures);
  }
}
