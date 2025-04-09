using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.StoreAggregate.Events;

public class FacilityInServiceEvent : DomainEventBase
{
  public Facility Facility { get; set; }

  public FacilityInServiceEvent(Facility facility)
  {
    Facility = Guard.Against.Null(facility, "facility");
  }
}
