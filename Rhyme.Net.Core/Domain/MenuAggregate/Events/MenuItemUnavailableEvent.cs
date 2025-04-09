using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Events;

public class MenuItemUnavailableEvent : DomainEventBase
{
  public Guid MenuId { get; set; }
  public MenuItem MenuItem { get; set; }

  public MenuItemUnavailableEvent(Guid brochureId, MenuItem menuItem)
  {
    MenuId = Guard.Against.Default(brochureId, "brochureId");
    MenuItem = Guard.Against.Null(menuItem, "menuItem");
  }
}