using Ardalis.Specification;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Specifications;

public class MenuForPosSpec : Specification<Menu>
{
  public MenuForPosSpec(Guid storeId)
  {
    Query
        .Where(menu => menu.StoreId == storeId)
        .Include(menu => menu.Brochures);
  }
}
