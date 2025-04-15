namespace Rhyme.Net.Core.EventSourcing;

public enum EventName
{
  OrderInitiated,
  OrderItemAdded,
  OrderItemRemoved,
  OrderItemQuantityAdjusted,
  OrderItemModifierAdjusted,
  OrderItemExtraAdjusted,
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