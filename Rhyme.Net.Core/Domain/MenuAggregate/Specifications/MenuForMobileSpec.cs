using Ardalis.Specification;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Specifications;

public class MenuForMobileSpec : Specification<Menu>
{
  public MenuForMobileSpec(Guid storeId)
  {
    Query
        .Where(menu => menu.StoreId == storeId)
        .Include(menu => menu.Brochures);
  }
}
