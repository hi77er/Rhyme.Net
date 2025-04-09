using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Events;

public class MenuItemPricingUpdatedEvent : DomainEventBase
{
  public MenuItem MenuItem { get; private set; }
  public Pricing? OldPricing { get; private set; }

  public MenuItemPricingUpdatedEvent(MenuItem menuItem, Pricing? oldPricing)
  {
    MenuItem = Guard.Against.Null(menuItem, "menuItem");
    OldPricing = oldPricing;
  }
}