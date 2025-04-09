using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.ProductAggregate;

namespace Rhyme.Net.Core.Domain.ProductAggregate.Events;

public class ProductNameUpdatedEvent : DomainEventBase
{
  public Product Product { get; private set; }
  public string OldName { get; private set; }

  public ProductNameUpdatedEvent(Product product, string oldName)
  {
    Product = Guard.Against.Null(product, "Product");
    OldName = Guard.Against.NullOrWhiteSpace(oldName, "oldName");
  }
}