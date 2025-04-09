using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class ModifierOption : SmartEnum<ModifierOption>
{
  public static readonly ModifierOption CoffeeTypeDecaf = new("Decafeinated", 0);
  public static readonly ModifierOption CoffeeTypeMediumRoast = new("Medium roast", 1);
  public static readonly ModifierOption CoffeeTypeSignitureBlend = new("Signiture blend", 2);
  // public static readonly ModifierOption DrinkType = new("Drink type", 1);
  // public static readonly ModifierOption Flavour = new("Flavour", 2);
  public static readonly ModifierOption HotOrColdHot = new("Hot", 3);
  public static readonly ModifierOption HotOrColdCold = new("Cold", 4);
  // public static readonly ModifierOption MilkSuffix = new("Milk suffix", 5);
  public static readonly ModifierOption MilkTypeCow = new("Cow milk", 5);
  public static readonly ModifierOption MilkTypeSoy = new("Soy milk", 5);
  public static readonly ModifierOption MilkTypeAlmond = new("Almond milk", 5);
  // public static readonly ModifierOption SaleOrNotSale = new("Sale or Not sale", 6);
  public static readonly ModifierOption SizeSmall = new("Small", 6);
  public static readonly ModifierOption SizeMedium = new("Medium", 7);
  public static readonly ModifierOption SizeBig = new("Big", 8);

  protected ModifierOption(string name, int value) : base(name, value) { }
}