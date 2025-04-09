using Ardalis.GuardClauses;
using Rhyme.Net.Core.Domain.OrderAggregate.Events;
using Ardalis.SharedKernel;

namespace Rhyme.Net.Core.Domain.OrderAggregate;

public class OrderItem : EntityBase<Guid>
{
  public string ProductName { get; set; } = string.Empty;
  public string ProductDescription { get; set; } = string.Empty;
  public decimal Price { get; private set; }

  public OrderItem(string productName, string? productDescription, decimal price)
  {
    ProductName = Guard.Against.NullOrWhiteSpace(productName, "productName");
    ProductDescription = productDescription ?? string.Empty;
    Price = Guard.Against.Negative(price, "price");
  }

  public override string ToString()
  {
    return $"Order Item Id: {Id}; Name: {ProductName}; Description: {ProductDescription}; Price: {Price.ToString("C")}";
  }
}
