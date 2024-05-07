namespace Shared
{
    public static class RabbitMQSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";
        public const string Payment_StockReservedEventQueue = "stock-reserved-event-queue";
        public const string Order_PaymentCompletedEventQueue = "payment-completed-event-queue";
        public const string Order_StockNotReservedEventQueue = "stock-notreserved-event-queue";
        public const string Order_PaymentFailedEventQueue= "payment-failed-event-queue";

    }
}
