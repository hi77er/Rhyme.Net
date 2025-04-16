using Ardalis.GuardClauses;
using Rhyme.Net.Core.Domain.OrderAggregate;

namespace Rhyme.Net.Core.Sourcing.Payloads;

public class AddOnAdjustedPayload
{
  public Guid? ItemId { get; set; }
  public SelectedAddOn? SelectedAddOn { get; set; }

  public AddOnAdjustedPayload() { }
  public AddOnAdjustedPayload(Guid itemId, SelectedAddOn selectedAddOn)
  {
    Guard.Against.Default(itemId, nameof(itemId));
    Guard.Against.Null(selectedAddOn, nameof(selectedAddOn));
    
    ItemId = itemId;
    SelectedAddOn = selectedAddOn;
  }
}
