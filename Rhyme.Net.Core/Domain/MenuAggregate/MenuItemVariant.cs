using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate;
using Rhyme.Net.Core.Domain.MenuAggregate.Events;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class MenuItemVariant : EntityBase<Guid>
{
  public Guid MenuItemId { get; private set; }
  public ModifierType ModifierType { get; private set; }
  public ModifierOption Option { get; private set; }
  public Pricing? VariantPricing { get; private set; }

  public MenuItemVariant(Guid menuItemId, ModifierType modifierType, ModifierOption option)
  {
    MenuItemId = Guard.Against.Expression(x => x == Guid.Empty, menuItemId, "menuItemId");
    ModifierType = Guard.Against.Null(modifierType, "modifierType");
    Option = Guard.Against.Null(option, "option");
  }

  public MenuItemVariant(Guid menuItemId, ModifierType modifierType, ModifierOption option, Pricing variantPricing)
  {
    MenuItemId = Guard.Against.Expression(x => x == Guid.Empty, menuItemId, "menuItemId");
    ModifierType = Guard.Against.Null(modifierType, "modifierType");
    Option = Guard.Against.Null(option, "option");
    VariantPricing = Guard.Against.Null(variantPricing, "variantPricing");
  }

  public void UpdateBasePricing(Pricing newVariantPricing)
  {
    VariantPricing = Guard.Against.Null(newVariantPricing, "newVariantPricing");
  }

  public override string ToString()
  {
    var price = VariantPricing?.Value ?? 0.0m;

    return $"Item Id: {Id}; rice: {price.ToString("C")};";
  }
}
