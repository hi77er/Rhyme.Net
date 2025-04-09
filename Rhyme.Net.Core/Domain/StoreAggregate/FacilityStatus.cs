using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.StoreAggregate;

public class FacilityStatus : SmartEnum<FacilityStatus>
{
  public static readonly FacilityStatus InService = new("In service", 0);
  public static readonly FacilityStatus InMaintanance = new("In maintanance", 1);
  public static readonly FacilityStatus OutOfService = new("Out of service", 2);

  protected FacilityStatus(string name, int value) : base(name, value) { }
}