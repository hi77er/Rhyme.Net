using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class ModifierType : SmartEnum<ModifierType>
{
  public static readonly ModifierType CoffeeType = new("Coffee type", 0);
  public static readonly ModifierType DrinkType = new("Drink type", 1);
  public static readonly ModifierType Flavour = new("Flavour", 2);
  public static readonly ModifierType HotOrCold = new("Hot or cold", 3);
  public static readonly ModifierType MilkSuffix = new("Milk suffix", 4);
  public static readonly ModifierType MilkType = new("Milk type", 5);
  public static readonly ModifierType SaleOrNotSale = new("Sale or Not sale", 6);
  public static readonly ModifierType Size = new("Size", 7);

  protected ModifierType(string name, int value) : base(name, value) { }
}