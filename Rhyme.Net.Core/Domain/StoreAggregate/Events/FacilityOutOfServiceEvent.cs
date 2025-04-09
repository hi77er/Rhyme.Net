using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.StoreAggregate.Events;

public class FacilityOutOfServiceEvent : DomainEventBase
{
  public Facility Facility { get; set; }

  public FacilityOutOfServiceEvent(Facility facility)
  {
    Facility = Guard.Against.Null(facility, "facility");
  }
}
