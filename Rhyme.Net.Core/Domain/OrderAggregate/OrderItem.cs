using Ardalis.GuardClauses;

namespace Rhyme.Net.Core.Domain.OrderAggregate;

public class OrderItem
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string ProductName { get; set; } = string.Empty;
  public string ProductDescription { get; set; } = string.Empty;
  public decimal Price { get; private set; }
  public IEnumerable<SelectedAddOn> SelectedAddOns { get; private set; } = new List<SelectedAddOn>();
  public IEnumerable<SelectedModifier> SelectedModifiers { get; private set; } = new List<SelectedModifier>();

  /// <summary>
  /// Default constructor for DynamoDB
  /// </summary>
  public OrderItem() { }

  public OrderItem(string productName, string? productDescription, decimal price)
  {
    ProductName = Guard.Against.NullOrWhiteSpace(productName, "productName");
    ProductDescription = productDescription ?? string.Empty;
    Price = Guard.Against.Negative(price, "price");
  }

  public void AdjustModifier(SelectedModifier selectedModifier)
  {
    var modifier = SelectedModifiers.FirstOrDefault(x => x.ModifierId == selectedModifier.ModifierId);
    if (modifier != null)
    {
      modifier.ModifierOption = selectedModifier.ModifierOption;
      modifier.ModifierType = selectedModifier.ModifierType;
      modifier.SelectedMenuItemVariantId = selectedModifier.SelectedMenuItemVariantId;
      SelectedModifiers = SelectedModifiers.Where(x => x.ModifierId != selectedModifier.ModifierId);
      SelectedModifiers = SelectedModifiers.Append(modifier);
    }
  }

  public void RemoveModifier(SelectedModifier selectedModifier)
  {
    var modifier = SelectedModifiers.FirstOrDefault(x => x.ModifierId == selectedModifier.ModifierId);
    if (modifier != null)
    {
      SelectedModifiers = SelectedModifiers.Where(x => x.ModifierId != selectedModifier.ModifierId);
    }
  }

  public void AdjustAddOn(SelectedAddOn selectedAddOn)
  {
    var addOn = SelectedAddOns.FirstOrDefault(x => x.AddOnId == selectedAddOn.AddOnId);
    if (addOn != null)
    {
      addOn.Quantity = selectedAddOn.Quantity;
      SelectedAddOns = SelectedAddOns.Where(x => x.AddOnId != selectedAddOn.AddOnId);
      SelectedAddOns = SelectedAddOns.Append(addOn);
    }
  }

  public void RemoveAddOn(SelectedAddOn selectedAddOn)
  {
    var addOn = SelectedAddOns.FirstOrDefault(x => x.AddOnId == selectedAddOn.AddOnId);
    if (addOn != null)
    {
      SelectedAddOns = SelectedAddOns.Where(x => x.AddOnId != selectedAddOn.AddOnId);
    }
  }
}
