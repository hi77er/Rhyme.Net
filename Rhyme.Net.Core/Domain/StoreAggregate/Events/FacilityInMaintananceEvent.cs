using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.StoreAggregate.Events;

public class FacilityInMaintananceEvent : DomainEventBase
{
  public Facility Facility { get; set; }

  public FacilityInMaintananceEvent(Facility facility)
  {
    Facility = Guard.Against.Null(facility, "facility");
  }
}
