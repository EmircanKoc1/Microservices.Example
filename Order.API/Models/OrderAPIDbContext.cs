using Microsoft.EntityFrameworkCore;
using APIEntities = Order.API.Models.Entities;

namespace Order.API.Models
{
    public class OrderAPIDbContext : DbContext
    {

        public OrderAPIDbContext(DbContextOptions<OrderAPIDbContext> options) : base(options)
        {

        }

        public DbSet<APIEntities.Order> Orders { get; set; }
        public DbSet<APIEntities.OrderItem> OrderItems { get; set; }







    }
}
