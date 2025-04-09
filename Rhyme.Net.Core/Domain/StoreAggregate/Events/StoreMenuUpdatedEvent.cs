using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;

namespace Rhyme.Net.Core.Domain.StoreAggregate.Events;

public class StoreMenuUpdatedEvent : DomainEventBase
{
  public Store Store { get; private set; }

  public StoreMenuUpdatedEvent(Store store)
  {
    Store = Guard.Against.Null(store, "store");
  }
}
