using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate.Events;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class Modifier : EntityBase<Guid>
{
  public ModifierType ModifierType { get; private set; }
  public IEnumerable<MenuItemVariant> Variants { get; private set; } = new List<MenuItemVariant>();
  public ModifierOption DefaultOption { get; private set; }

  public Modifier(ModifierType modifierType, ModifierOption defaultOption)
  {
    ModifierType = modifierType;
    DefaultOption = Guard.Against.Null(defaultOption, "defaultOption");
  }

  public Modifier(ModifierType modifierType, IEnumerable<MenuItemVariant> variants, ModifierOption defaultOption)
  {
    ModifierType = modifierType;
    Variants = Guard.Against.Null(variants, "variants");
    DefaultOption = defaultOption;
  }

  public override string ToString()
  {
    return $"Modifier: {ModifierType.Name}; Variants count: {Variants.Count()}; Default: {DefaultOption.Name};";
  }
}
