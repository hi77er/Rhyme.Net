using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.StoreAggregate.Events;

namespace Rhyme.Net.Core.Domain.StoreAggregate;

public class Facility : EntityBase<Guid>
{
  public string Name { get; private set; }
  public string Description { get; private set; }
  public string Size { get; private set; }
  public string Category { get; private set; }
  public Guid StoreId { get; private set; }
  public FacilityStatus Status { get; private set; } = FacilityStatus.InService;

  public Facility(string name, string? description, string size, string category, Guid storeId)
  {
    Name = Guard.Against.NullOrWhiteSpace(name, "name");
    Description = description ?? string.Empty;
    Size = Guard.Against.NullOrWhiteSpace(size, "size");
    Category = Guard.Against.NullOrWhiteSpace(category, "category");
    StoreId = Guard.Against.Expression(x => x == Guid.Empty, storeId, "storeId");
  }

  public void MarkOutOfService()
  {
    Status = FacilityStatus.OutOfService;
    RegisterDomainEvent(new FacilityOutOfServiceEvent(this));
  }

  public void MarkInMaintananceService()
  {
    Status = FacilityStatus.InMaintanance;
    RegisterDomainEvent(new FacilityInMaintananceEvent(this));
  }

  public void MarkInOfService()
  {
    Status = FacilityStatus.InService;
    RegisterDomainEvent(new FacilityInServiceEvent(this));
  }

  public override string ToString()
  {
    return $"Facility: {Name}; Facility: {Description}; Status: {Status.Name};";
  }
}
