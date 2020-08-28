namespace Order.Infrastructure.Data
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Order = Core.Entities.Order;
    public class OrderContextSeed
    {
        public static async Task SeedDataAsync(OrderContext orderContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                orderContext.Database.Migrate();

                if (!orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetOrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 5)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError($"Exception occured while connecting: {ex.Message}");
                    await SeedDataAsync(orderContext, loggerFactory, retryForAvailability);
                }
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new Order()
                {
                    UserName = "rahulsahay",
                    AddressLine = "Ranchi",
                    CardName = "Rahul Sahay",
                    CardNumber = "1234567890",
                    Country = "India",
                    CVV = "123",
                    EmailAddress = "rahul@sahay.com",
                    Expiration = "12/24",
                    FirstName = "Rahul",
                    LastName = "Sahay",
                    State = "Jharkhand",
                    ZipCode = "834002",
                    PaymentMethod = 1,
                    TotalPrice = 970
                }
            };
        }
    }
}
