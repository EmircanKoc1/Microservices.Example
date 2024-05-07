
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;
using Shared;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<OrderAPIDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<PaymentCompletedEventConsumer>();
    configurator.AddConsumer<StockNotReservedEventConsumer>();
    configurator.AddConsumer<PaymentFailedEventConsumer>();

    configurator.UsingRabbitMq((_context, _configurator) =>
    {
        _configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));


        _configurator.ReceiveEndpoint(
            queueName: RabbitMQSettings.Order_PaymentCompletedEventQueue,
            configureEndpoint: e =>
            {
                e.ConfigureConsumer<PaymentCompletedEventConsumer>(_context);
            });

        _configurator.ReceiveEndpoint(
            queueName: RabbitMQSettings.Order_StockNotReservedEventQueue,
            configureEndpoint: e =>
            {
                e.ConfigureConsumer<StockNotReservedEventConsumer>(_context);
            });

        _configurator.ReceiveEndpoint(
            queueName: RabbitMQSettings.Order_PaymentFailedEventQueue,
            configureEndpoint: e =>
            {
                e.ConfigureConsumer<PaymentFailedEventConsumer>(_context);
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
