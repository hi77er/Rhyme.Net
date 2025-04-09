using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.ProductAggregate;

public class SensitivitiesContainType : SmartEnum<SensitivitiesContainType>
{
  public static readonly SensitivitiesContainType KnownToContain = new("Known to contain", 0);
  public static readonly SensitivitiesContainType MayContain = new("May contain", 1);
  public static readonly SensitivitiesContainType DoesNotContain = new("Does not contain", 2);

  protected SensitivitiesContainType(string name, int value) : base(name, value) { }
}