namespace Order.Infrastructure.Data
{
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options): base(options)
        {
            
        }

        public DbSet<Order> Orders { get; set; }
    }
}
