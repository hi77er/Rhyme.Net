using Ardalis.GuardClauses;

namespace Rhyme.Net.Core.Sourcing.Payloads;

public class OrderInitiatedPayload
{
  public Guid? OrderId { get; set; }
  public Guid? StoreId { get; set; }

  public OrderInitiatedPayload() { }
  public OrderInitiatedPayload(Guid orderId, Guid storeId)
  {
    Guard.Against.NullOrEmpty(orderId, "orderId");
    Guard.Against.NullOrEmpty(storeId, "storeId");
    
    OrderId = orderId;
    StoreId = storeId;
  }
}
