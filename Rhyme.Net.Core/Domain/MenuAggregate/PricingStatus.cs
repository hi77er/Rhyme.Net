using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class PricingStatus : SmartEnum<PricingStatus>
{
  public static readonly PricingStatus Approved = new("Approved", 0);
  public static readonly PricingStatus NotApproved = new("Not approved", 1);

  protected PricingStatus(string name, int value) : base(name, value) { }
}