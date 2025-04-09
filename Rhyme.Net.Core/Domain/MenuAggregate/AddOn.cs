using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate;
using Rhyme.Net.Core.Domain.MenuAggregate.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class AddOn : EntityBase<Guid>
{
  public string Name { get; private set; }
  public string Description { get; private set; } = string.Empty;
  public Guid MenuItemId { get; private set; }
  public Guid ProductId { get; private set; }
  public Product? Product { get; private set; }
  public AddOnMeasureType MeasureType { get; private set; }
  public bool IsAvailable { get; private set; }
  public Pricing? AddOnPricing { get; private set; }

  public AddOn(string name, Guid menuItemId, Guid productId, AddOnMeasureType measureType)
  {
    Name = Guard.Against.NullOrWhiteSpace(name, "name");
    MenuItemId = Guard.Against.Default(menuItemId, "menuItemId");
    ProductId = Guard.Against.Default(productId, "productId");
    MeasureType = Guard.Against.Null(measureType, "unitOfMeasure");
  }

  public AddOn(string name, string? description, Guid menuItemId, Guid productId, AddOnMeasureType measureType, Pricing addOnPricing)
  {
    Name = Guard.Against.NullOrWhiteSpace(name, "name");
    Description = description ?? string.Empty;
    ProductId = Guard.Against.Default(productId, "productId");
    MenuItemId = Guard.Against.Default(menuItemId, "menuItemId");
    AddOnPricing = Guard.Against.Null(addOnPricing, "addOnPricing");
    MeasureType = Guard.Against.Null(measureType, "unitOfMeasure");
  }

  public void SetProduct(Product product)
  {
    Product = Guard.Against.Null(product, "product");
  }
  public void MarkUnavailable()
  {
    if (IsAvailable)
    {
      IsAvailable = false;

      var domainEvent = new AddOnUnavailableEvent(this);
      RegisterDomainEvent(domainEvent);
    }
  }

  public override string ToString()
  {
    string status = IsAvailable ? "Available" : "Unavailable";
    return $"Add on: {Name}; Description: {Description}; Price: {(AddOnPricing?.Value.ToString("C")): Not set}; Status: {status}";
  }
}
