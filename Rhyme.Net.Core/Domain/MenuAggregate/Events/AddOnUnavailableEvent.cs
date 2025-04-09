using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.MenuAggregate.Events;

public class AddOnUnavailableEvent : DomainEventBase
{
  public AddOn AddOn { get; set; }

  public AddOnUnavailableEvent(AddOn addOn)
  {
    AddOn = Guard.Against.Null(addOn, "addOn");
  }
}
