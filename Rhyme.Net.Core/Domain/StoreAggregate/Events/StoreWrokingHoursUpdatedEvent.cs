using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.StoreAggregate.Events;

public class StoreWorkingHoursUpdatedEvent : DomainEventBase
{
  public Store Store { get; set; }
  public string NewWorkingHours { get; set; }

  public StoreWorkingHoursUpdatedEvent(Store store, string newWorkingHours)
  {
    Store = store;
    NewWorkingHours = newWorkingHours;
  }
}
