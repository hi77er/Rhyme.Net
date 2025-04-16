namespace Rhyme.Net.Core.Domain.OrderAggregate;

public class SelectedAddOn
{
  public Guid? AddOnId { get; set; }
  public int Quantity { get; set; }
  
  public SelectedAddOn() {}

}
