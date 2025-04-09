using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate;
using Rhyme.Net.Core.Domain.MenuAggregate.Events;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class MenuItem : EntityBase<Guid>
{
  public Guid BrochureId { get; private set; }
  public Guid ProductId { get; private set; }
  public Product? Product { get; private set; }
  public string Description { get; private set; }
  public Pricing? DefaultPricing { get; private set; }
  public bool IsAvailable { get; private set; }
  public IEnumerable<Modifier> Modifiers { get; private set; } = new List<Modifier>();
  public IEnumerable<AddOn> AddOns { get; private set; } = new List<AddOn>();

  public MenuItem(Guid brochureId, Guid productId)
  {
    BrochureId = Guard.Against.Default(brochureId, "brochureId");
    ProductId = Guard.Against.Default(productId, "productId");
    Description = string.Empty;
    IsAvailable = false;
  }

  public MenuItem(Guid brochureId, Guid productId, string? description, bool isAvailable, Pricing defaultPricing, IEnumerable<Modifier> modifiers, IEnumerable<AddOn> addOns)
  {
    BrochureId = Guard.Against.Default(brochureId, "brochureId");
    ProductId = Guard.Against.Default(productId, "productId");
    Description = description ?? string.Empty;
    IsAvailable = isAvailable;
    DefaultPricing = Guard.Against.Null(defaultPricing, "defaultPricing");
    Modifiers = Guard.Against.Null(modifiers, "modifiers");
    AddOns = Guard.Against.Null(addOns, "addOns");
  }

  public void UpdateDefaultPricing(Pricing newDefaultPricing)
  {
    var oldDefaultPricing = DefaultPricing;
    DefaultPricing = Guard.Against.Null(newDefaultPricing, "newDefaultPricing");

    var domainEvent = new MenuItemPricingUpdatedEvent(this, oldDefaultPricing);
    RegisterDomainEvent(domainEvent);
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

      var domainEvent = new MenuItemUnavailableEvent(BrochureId, this);
      RegisterDomainEvent(domainEvent);
    }
  }

  public override string ToString()
  {
    var status = IsAvailable ? "Available" : "Unavailable";
    var price = DefaultPricing?.Value ?? 0.0m;

    return $"Item Id: {Id}; Product Id: {ProductId}; Price: {price.ToString("C")}; Status: {status}";
  }
}
