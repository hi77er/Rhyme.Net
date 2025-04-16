namespace Rhyme.Net.Core.Sourcing;

public enum EventName
{
  OrderInitiated,
  OrderItemAdded,
  OrderItemRemoved,
  OrderItemModifierAdjusted,
  OrderItemModifierRemoved,
  OrderItemAddOnAdjusted,
  OrderItemAddOnRemoved,

  OrderVoucherSelected,
  OrderVoucherApplied,
  OrderPromoApplied,
  OrderPaymentMethodSelected,

  OrderSubmitted,
  
  OrderPaymentSubmitted,
  OrderPaymentReceived,
  OrderPreparationStarted,
  
  OrderCompleted,
  
  OrderDeliveryStarted,
  OrderDelivered,

}