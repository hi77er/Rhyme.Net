using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate.Events;

namespace Rhyme.Net.Core.Domain.MenuAggregate;

public class Pricing : EntityBase<Guid>
{
  public string PriceBand { get; private set; }
  public string Currency { get; private set; }
  public decimal Value { get; private set; }
  public Guid ProductId { get; private set; }
  public Guid ChannelId { get; private set; }
  public DateTime EffectiveFrom { get; private set; }
  public PricingStatus Status { get; private set; } = PricingStatus.NotApproved;

  public Pricing(string priceBand, string currency, decimal value, Guid productId, Guid channelId, DateTime effectiveFrom)
  {
    PriceBand = Guard.Against.NullOrWhiteSpace(priceBand, "priceBand");
    Value = Guard.Against.Negative(value, "value");
    Currency = Guard.Against.NullOrWhiteSpace(currency, "currency");
    ProductId = Guard.Against.Expression(x => x == Guid.Empty, productId, "productId");
    ChannelId = Guard.Against.Expression(x => x == Guid.Empty, channelId, "channelId");
    EffectiveFrom = Guard.Against.Expression(x => x < DateTime.UtcNow, effectiveFrom, "effectiveFrom");
  }

  public void Approve()
  {
    if (Status == PricingStatus.NotApproved)
      Status = PricingStatus.Approved;
  }

  public override string ToString()
  {
    return $"Price band: {PriceBand}; Value: {Value.ToString("C")}; Channel: {ChannelId}, Product: {ProductId}; Effective from: {EffectiveFrom.ToString("yyyy-MM-dd hh:mm")}; Status: {Status.Name};";
  }
}
