using MassTransit;
using MongoDB.Driver;
using Shared;
using Stock.API.Consumers;
using Stock.API.Services;

namespace Stock.API
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
                configurator.AddConsumer<OrderCreatedEventConsumer>();

                configurator.UsingRabbitMq((_context, _configurator) =>
                {

                    _configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

                    _configurator.ReceiveEndpoint(
                        queueName: RabbitMQSettings.Stock_OrderCreatedEventQueue,
                        configureEndpoint: e =>
                        {
                            e.ConfigureConsumer<OrderCreatedEventConsumer>(_context);
                        });

                });


            });

            builder.Services.AddSingleton<MongoDBService>();

            using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();

            MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();

            var collection = mongoDBService.GetCollection<Stock.API.Entities.Stock>();

            if (!collection.FindAsync(Builders<Stock.API.Entities.Stock>.Filter.Empty).Result.Any())
            {
                collection.InsertOne(new Entities.Stock() { ProductId = Guid.NewGuid(), Count = 2000 });
                collection.InsertOne(new Entities.Stock() { ProductId = Guid.NewGuid(), Count = 1000 });
                collection.InsertOne(new Entities.Stock() { ProductId = Guid.NewGuid(), Count = 3000 });
                collection.InsertOne(new Entities.Stock() { ProductId = Guid.NewGuid(), Count = 5000 });
                collection.InsertOne(new Entities.Stock() { ProductId = Guid.NewGuid(), Count = 500 });

            }


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
