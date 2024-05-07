using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            bool flag = true;

            if (flag)
            {
                PaymentCompletedEvent paymentCompletedEvent = new PaymentCompletedEvent()
                {
                    OrderId = context.Message.OrderId
                };

                await _publishEndpoint.Publish<PaymentCompletedEvent>(paymentCompletedEvent);

            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new PaymentFailedEvent()
                {
                    OrderId = context.Message.OrderId,
                    Message = "ödeme işlemi başarısız"
                };

                await _publishEndpoint.Publish<PaymentFailedEvent>(paymentFailedEvent);


            }

        }
    }
}
