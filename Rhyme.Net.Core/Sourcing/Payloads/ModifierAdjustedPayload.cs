using Ardalis.GuardClauses;
using Rhyme.Net.Core.Domain.OrderAggregate;

namespace Rhyme.Net.Core.Sourcing.Payloads;

public class ModifierAdjustedPayload
{
  public Guid? ItemId { get; set; }
  public SelectedModifier? SelectedModifier { get; set; }

  public ModifierAdjustedPayload() { }
  public ModifierAdjustedPayload(Guid itemId, SelectedModifier selectedModifier)
  {
    Guard.Against.Default(itemId, nameof(itemId));
    Guard.Against.Null(selectedModifier, nameof(selectedModifier));
    
    ItemId = itemId;
    SelectedModifier = selectedModifier;
  }
}
