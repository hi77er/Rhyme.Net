using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Events;

public class MenuTitleUpdatedEvent : DomainEventBase
{
  public Menu Menu { get; private set; }
  public string OldTitle { get; private set; }

  public MenuTitleUpdatedEvent(Menu menu, string oldTitle)
  {
    Menu = Guard.Against.Null(menu, "menu");
    OldTitle = Guard.Against.NullOrWhiteSpace(oldTitle, "oldTitle");
  }
}