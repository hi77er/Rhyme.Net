using Ardalis.SmartEnum;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class AddOnMeasureType : SmartEnum<AddOnMeasureType>
{
  public static readonly AddOnMeasureType Portion = new("Initiated", 0);
  public static readonly AddOnMeasureType Piece = new("Piece", 1);
  public static readonly AddOnMeasureType Bottle = new("Submitted", 2);
  public static readonly AddOnMeasureType Spoon = new("Spoon", 3);
  public static readonly AddOnMeasureType Cup = new("Cup", 4);
  public static readonly AddOnMeasureType Glass = new("Glass", 5);
  public static readonly AddOnMeasureType Mililiter = new("Milliliter", 6);
  public static readonly AddOnMeasureType Litre = new("Litre", 7);
  public static readonly AddOnMeasureType Gram = new("Gram", 8);

  protected AddOnMeasureType(string name, int value) : base(name, value) { }
}