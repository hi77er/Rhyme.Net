using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.ProductAggregate;

public class AllergensContainType : SmartEnum<AllergensContainType>
{
  public static readonly AllergensContainType KnownToContain = new("Known to contain", 0);
  public static readonly AllergensContainType MayContain = new("May contain", 1);
  public static readonly AllergensContainType DoesNotContain = new("Does not contain", 2);

  protected AllergensContainType(string name, int value) : base(name, value) { }
}