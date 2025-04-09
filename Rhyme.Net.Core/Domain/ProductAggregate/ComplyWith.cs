using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.ProductAggregate;

public class ComplyWith : SmartEnum<ComplyWith>
{
  public static readonly ComplyWith ContainsAnimalDerivatives = new("Contains aminal derivatives", 0);
  public static readonly ComplyWith Vegan = new("Vegan", 1);
  public static readonly ComplyWith Vegetarian = new("Vegetarian", 2);
  public static readonly ComplyWith HalalCertified = new("Halal certified", 3);
  public static readonly ComplyWith KosherCertified = new("Kosher sertified", 4);

  protected ComplyWith(string name, int value) : base(name, value) { }
}