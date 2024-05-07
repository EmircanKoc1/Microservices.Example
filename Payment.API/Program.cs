
using MassTransit;
using Payment.API.Consumers;
using Shared;

namespace Payment.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddMassTransit(configurator =>
            {
                configurator.AddConsumer<StockReservedEventConsumer>();

                configurator.UsingRabbitMq((_context, _configurator) =>
                {

                    _configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

                    _configurator.ReceiveEndpoint(
                        queueName: RabbitMQSettings.Payment_StockReservedEventQueue,
                        configureEndpoint: e =>
                        {
                            e.ConfigureConsumer<StockReservedEventConsumer>(_context);
                        });

                });



            });



            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
