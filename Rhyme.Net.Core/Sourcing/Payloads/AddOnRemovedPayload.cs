using Ardalis.GuardClauses;
using Rhyme.Net.Core.Domain.OrderAggregate;

namespace Rhyme.Net.Core.Sourcing.Payloads;

public class AddOnRemovedPayload
{
  public Guid? ItemId { get; set; }
  public SelectedAddOn? SelectedAddOn { get; set; }

  public AddOnRemovedPayload() { }
  public AddOnRemovedPayload(Guid itemId, SelectedAddOn selectedAddOn)
  {
    Guard.Against.Default(itemId, nameof(itemId));
    Guard.Against.Null(selectedAddOn, nameof(selectedAddOn));
    
    ItemId = itemId;
    SelectedAddOn = selectedAddOn;
  }
}
