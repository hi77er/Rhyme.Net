using Rhyme.Net.Core.Domain.MenuAggregate;

namespace Rhyme.Net.Core.Domain.OrderAggregate;

public class SelectedModifier
{
  public Guid? ModifierId { get; set; }
  public ModifierType? ModifierType { get; set; }

  public Guid? SelectedMenuItemVariantId { get; set; }
  public ModifierOption? ModifierOption { get; set; }

  public SelectedModifier() { }

}
