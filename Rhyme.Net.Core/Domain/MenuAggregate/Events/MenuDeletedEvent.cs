using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Events;

public class MenuDeletedEvent : DomainEventBase
{
  public Menu Menu { get; private set; }

  public MenuDeletedEvent(Menu menu)
  {
    Menu = Guard.Against.Null(menu, "menu");
  }
}