using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;

namespace Rhyme.Net.Core.Domain.StoreAggregate.Events;

public class StoreNameUpdatedEvent : DomainEventBase
{
  public Store Store { get; private set; }
  public string OldName { get; private set; }

  public StoreNameUpdatedEvent(Store store, string oldName)
  {
    Store = Guard.Against.Null(store, "store");
    OldName = Guard.Against.NullOrWhiteSpace(oldName, "oldName");
  }
}